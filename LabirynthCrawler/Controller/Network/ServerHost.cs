using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using LabirynthCrawler.Controller.InputHandling;
using LabirynthCrawler.Model;
using LabirynthCrawler.Model.Board;
using LabirynthCrawler.Model.Logger;
using LabirynthCrawler.Model.Network;
using LabirynthCrawler.View.Renderers;

namespace LabirynthCrawler.Controller.Network;

public class ServerHost
{
    private readonly int _port;
    private readonly GameConfig _config;
    private readonly GameModel _model;
    
    private TcpListener _listener;
    private bool _isRunning = true;
    
    private int _nextPlayerId = 2;
    
    private readonly Dictionary<int, StreamWriter> _clients = new();
    private readonly object _clientsLock = new object();
    
    private BaseHandler _actionChain;

    public ServerHost(int port, GameConfig config)
    {
        _port = port;
        _config = config;
        _model = new GameModel();
    }

    public void Start()
    {
        _model.InitializeGame(21, 11, _config.Theme);

        _listener = new TcpListener(IPAddress.Any, _port);
        _listener.Start();
        GameLogger.Instance.Log($"Listening on port {_port} active.", LogLevel.System);

        Task.Run(() => AcceptClientsLoop());

        int localPlayerId = 1;
        MainHandler handler = new MainHandler();
        handler.Initialize();
        _actionChain = handler.GetStartHandler();
        
        
        LocalRenderer renderer = new LocalRenderer();
        renderer.Initialize();
        renderer.Render(_model, localPlayerId);


        handler.RunGame(_model, renderer, localPlayerId, BroadcastState);
        
        _isRunning = false;
        _listener.Stop();
    }

    private void AcceptClientsLoop()
    {
        while (_isRunning)
        {
            try
            {
                TcpClient client = _listener.AcceptTcpClient();
                
                lock (_clientsLock)
                {
                    int newPlayerId = GetAvailablePlayerId();
                    if (newPlayerId == -1 || _clients.Count >= 8)
                    {
                        client.Close();
                        continue;
                    }

                    NetworkStream stream = client.GetStream();
                    StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };
                    
                    _clients.Add(newPlayerId, writer);
                    
                    lock (_model.StateLock)
                    {
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
                    GameLogger.Instance.Log($"Player {newPlayerId} joined The Game.", LogLevel.System);
                    
                    BroadcastState();
                    
                    Task.Run(() => ListenToClient(client, newPlayerId));
                }
            }
            catch (Exception)
            {
                if (!_isRunning) break;
            }
        }
    }

    private void ListenToClient(TcpClient client, int playerId)
    {
        using StreamReader reader = new StreamReader(client.GetStream());
        try
        {
            while (_isRunning && client.Connected)
            {
                string? json = reader.ReadLine();
                if (json == null) break;

                var action = JsonSerializer.Deserialize<PlayerActionDto>(json);
                if (action != null)
                {
                    ProcessAction(action);
                }
            }
        }
        catch (Exception) { /* Ignoring disconnection */ }
        finally
        {
            client.Close();
            lock (_clientsLock)
            {
                _clients.Remove(playerId);
                _model.RemovePlayer(playerId);
            }
            

            GameLogger.Instance.Log($"Player {playerId} left The Game.", LogLevel.System);
            BroadcastState();
        }
    }

    private void ProcessAction(PlayerActionDto action)
    {
        lock (_model.StateLock)
        {
            _actionChain.Handle(action, _model);
        }
    
        BroadcastState();
    }
    private void BroadcastState()
    {
        lock (_clientsLock)
        {
            List<int> disconnected = new();
            foreach (var kvp in _clients)
            {
                try
                {
                    GameStateDto dto = StateMapper.ToDto(_model, kvp.Key);
                    string json = JsonSerializer.Serialize(dto);
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
        for (int i = 2; i <= 9; i++)
        {
            if (!_clients.ContainsKey(i)) return i;
        }
        return -1;
    }
}