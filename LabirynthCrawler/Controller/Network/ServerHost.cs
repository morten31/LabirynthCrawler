using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Diagnostics;
using LabirynthCrawler.Controller.InputHandling;
using LabirynthCrawler.Model;
using LabirynthCrawler.Model.Board;
using LabirynthCrawler.Model.Logger;
using LabirynthCrawler.Model.Network;
using LabirynthCrawler.View.Renderers;

namespace LabirynthCrawler.Controller.Network;

public class ServerHost
{
    private enum ServerState { Lobby, Playing, GameOver }
    
    private readonly int _port;
    private readonly GameConfig _config;
    private readonly GameModel _model;
    
    private TcpListener _listener;
    private ServerState _serverState = ServerState.Lobby;
    
    private readonly Dictionary<int, StreamWriter> _clients = new();
    private readonly object _clientsLock = new object();
    
    // Event Queue
    private readonly ConcurrentQueue<(int PlayerId, PlayerActionDto Action)> _actionQueue = new();
    
    private BaseHandler _actionChain;

    public ServerHost(int port, GameConfig config)
    {
        _port = port;
        _config = config;
        _model = new GameModel();
    }

    public async Task StartAsync()
    {
        _model.InitializeGame(21, 11, _config.Theme);

        var handler = new MainHandler();
        handler.Initialize();
        _actionChain = handler.GetStartHandler();

        _listener = new TcpListener(IPAddress.Any, _port);
        _listener.Start();
        
        Console.WriteLine($"[SERVER] Listening on port {_port}. Waiting for players...");
        Console.WriteLine("[SERVER] Press 'S' to start the game when all players are ready.");

        _ = Task.Run(AcceptClientsLoop);

        // 1. Lobby
        while (_serverState == ServerState.Lobby)
        {
            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.S)
            {
                _serverState = ServerState.Playing;
                Console.WriteLine("[SERVER] Game Started! No more players can join.");
                GameLogger.Instance.Log("Game started by the Host.", LogLevel.System);
            }
            await Task.Delay(50);
        }

        int hostPlayerId = 1;
        LocalRenderer hostRenderer = new LocalRenderer();
        LocalInputManager hostInput = new LocalInputManager();
        hostRenderer.Initialize();
        
        // 2. Game Loop
        int tickRateMs = 50; 
        
        while (_serverState == ServerState.Playing)
        {
            var stopwatch = Stopwatch.StartNew();
            bool stateChanged = false;
            PlayerActionDto? actionToSend = null;

            while (Console.KeyAvailable)
            {
                var keyInfo = Console.ReadKey(true);
            
                var action = hostInput.ProcessInput(keyInfo, hostPlayerId, _model.CurrentState.ToString());
                
                if (action != null)
                {
                    actionToSend = action;
                }
            }
            
            if (actionToSend != null)
            {
                if (actionToSend.ActionType == "Quit") break;
                _actionQueue.Enqueue((hostPlayerId, actionToSend));
            }
            
            while (_actionQueue.TryDequeue(out var item))
            {
                lock (_model.StateLock)
                {
                    _actionChain.Handle(item.Action, _model);
                }
                stateChanged = true;
            }

            lock (_model.StateLock)
            {
                _model.Tick();
                if (_model.CurrentState == GameModel.GameState.GameOver)
                {
                    _serverState = ServerState.GameOver;
                    stateChanged = true;
                }
            }

            await BroadcastStateAsync();

            hostRenderer.Render(_model, hostPlayerId);

            int elapsed = (int)stopwatch.ElapsedMilliseconds;
            if (elapsed < tickRateMs)
            {
                await Task.Delay(tickRateMs - elapsed);
            }
        }
        
        Console.Clear();
        Console.WriteLine("[SERVER] Game Over.");
        _listener.Stop();
    }

    private async Task AcceptClientsLoop()
    {
        while (_serverState == ServerState.Lobby)
        {
            try
            {
                if (!_listener.Pending())
                {
                    await Task.Delay(100);
                    continue;
                }

                TcpClient client = await _listener.AcceptTcpClientAsync();
                
                lock (_clientsLock)
                {
                    if (_serverState != ServerState.Lobby || _clients.Count >= 8)
                    {
                        client.Close();
                        continue;
                    }

                    int newPlayerId = GetAvailablePlayerId();
                    NetworkStream stream = client.GetStream();
                    StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };
                    
                    _clients.Add(newPlayerId, writer);
                    
                    lock (_model.StateLock)
                    {
                        // TODO: Znalezienie losowego wolnego miejsca niedaleko (21, 11) 
                        _model.AddPlayer(newPlayerId, 21, 11); 
                    }

                    var welcome = new WelcomeDto { 
                        AssignedPlayerId = newPlayerId, 
                        MapWidth = Map.Width, 
                        MapHeight = Map.Height 
                    };
                    for (int y = 0; y < Map.Height; y++)
                    {
                        for (int x = 0; x < Map.Width; x++)
                        {
                            if (_model.GetMap().GetTile(x, y).IsWall())
                                welcome.StaticWalls.Add(new TileDto { X = x, Y = y, Symbol = "█", IsWall = true });
                        }
                    }
                    writer.WriteLine(JsonSerializer.Serialize(welcome));
                    
                    Console.WriteLine($"[SERVER] Player {newPlayerId} joined the lobby.");
                    
                    _ = Task.Run(() => ListenToClientAsync(client, newPlayerId));
                }
            }
            catch (Exception) { /* ignore */ }
        }
    }

    private async Task ListenToClientAsync(TcpClient client, int playerId)
    {
        using StreamReader reader = new StreamReader(client.GetStream());
        try
        {
            while (client.Connected)
            {
                string? json = await reader.ReadLineAsync();
                if (json == null) break;

                var action = JsonSerializer.Deserialize<PlayerActionDto>(json);
                if (action != null)
                {
                    _actionQueue.Enqueue((playerId, action));
                }
            }
        }
        catch (Exception) { /* Disconnect */ }
        finally
        {
            client.Close();
            lock (_clientsLock)
            {
                _clients.Remove(playerId);
            }
            lock (_model.StateLock)
            {
                var p = _model.GetPlayer(playerId);
                if (p != null) p.IsDead = true;
            }
            Console.WriteLine($"[SERVER] Player {playerId} disconnected.");
        }
    }

    private async Task BroadcastStateAsync()
    {
        var updateDto = new UpdateDto();
    
        lock (_model.StateLock)
        {
            updateDto.CurrentState = _model.CurrentState.ToString();
        
            while (GameLogger.Instance.NetworkEventQueue.TryDequeue(out var logEntry))
            {
                updateDto.NewEvents.Add(logEntry);
            }

            foreach (var kvp in _model.Players)
            {
                var p = kvp.Value;
                var attr = p.GetTotalAttributes();
                var hands = p.GetInventory().GetHandsContent();
                
                updateDto.Players[kvp.Key] = new PlayerDto
                {
                    Id = kvp.Key,
                    X = p.GetX(), Y = p.GetY(),
                    IsDead = p.IsDead,
                    Health = attr.Health, Power = attr.Power, Agility = attr.Agility, 
                    Luck = attr.Luck, Aggression = attr.Aggression, Wisdom = attr.Wisdom,
                    Coins = p.GetInventory().GetCoinsCount(),
                    Gold = p.GetInventory().GetGoldCount(),
                    Inventory = p.GetInventory().GetItems().Select(i => i.ToString()).ToList(),
                    LeftHand = hands.Item1?.ToString() ?? "Empty",
                    RightHand = hands.Item2?.ToString() ?? "Empty",
                    IsViewingLog = p.IsViewingLog
                };;
            }
        
            for (int y = 0; y < Map.Height; y++)
            {
                for (int x = 0; x < Map.Width; x++)
                {
                    var tile = _model.GetMap().GetTile(x, y);
                    if (!tile.IsWall() && tile.GetSymbol() != " ")
                    {
                        updateDto.DynamicTiles.Add(new TileDto { X = x, Y = y, Symbol = tile.GetSymbol() });
                    }
                }
            }
        }

        string json = JsonSerializer.Serialize(updateDto);
        
        List<int> disconnected = new();
        lock (_clientsLock)
        {
            foreach (var kvp in _clients)
            {
                try
                {
                    kvp.Value.WriteLine(json);
                }
                catch
                {
                    disconnected.Add(kvp.Key);
                }
            }
            foreach (var id in disconnected) _clients.Remove(id);
        }
    }
    
    private int GetAvailablePlayerId()
    {
        for (int i = 2; i <= 9; i++) if (!_clients.ContainsKey(i)) return i;
        return -1;
    }
}