using System.Net.Sockets;
using System.Text.Json;
using LabirynthCrawler.Controller.InputHandling;
using LabirynthCrawler.Model.Logger;
using LabirynthCrawler.Model.Network;
using LabirynthCrawler.View.Renderers;

namespace LabirynthCrawler.Controller.Network;

public class ClientHost
{
    private readonly string _ip;
    private readonly int _port;
    private int _myPlayerId;
    private ClientRenderer _renderer;
    private bool _isRunning = true;
    private string _currentState = "Lobby";
    private LocalInputManager _inputManager = new();
    private List<LogEntry> _localLogs = new();
    private bool _isViewingLog = false;
    private UpdateDto? _lastUpdate = null;
    
    public ClientHost(string ip, int port)
    {
        _ip = ip;
        _port = port;
        _renderer = new ClientRenderer();
    }

    public async Task StartAsync()
    {
        Console.Clear();
        Console.WriteLine($"Connecting to server {_ip}:{_port}...");

        try
        {
            using TcpClient client = new TcpClient(_ip, _port);
            using NetworkStream stream = client.GetStream();
            using StreamReader reader = new StreamReader(stream);
            using StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

            string? welcomeJson = await reader.ReadLineAsync();
            if (welcomeJson != null)
            {
                var welcome = JsonSerializer.Deserialize<WelcomeDto>(welcomeJson);
                if (welcome != null)
                {
                    _myPlayerId = welcome.AssignedPlayerId;
                    Console.WriteLine($"Connected! You are player {_myPlayerId}. Waiting for server host to start...");
                    _renderer.Initialize(welcome);
                }
            }

            _ = Task.Run(async () =>
            {
                try
                {
                    while (_isRunning)
                    {
                        string? stateJson = await reader.ReadLineAsync();
                        if (stateJson == null) break;

                        var updateDto = JsonSerializer.Deserialize<UpdateDto>(stateJson);
                        if (updateDto != null)
                        {        
                            _currentState = updateDto.CurrentState;
                            _lastUpdate = updateDto; 
                            if (updateDto.NewEvents.Any())
                            {
                                _localLogs.AddRange(updateDto.NewEvents);
                                if (_localLogs.Count > 50) _localLogs.RemoveRange(0, _localLogs.Count - 50);
                            }
                            
                            _renderer.Render(updateDto, _localLogs, _myPlayerId, _isViewingLog);
                        }
                    }
                }
                catch (Exception) { /* Server closed */ }
                _isRunning = false;
            });

            // Read input
            while (_isRunning)
            {
                PlayerActionDto? actionToSend = null;
                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    
                    if (KeyBindings.Matches(key, GameAction.Quit))
                    {
                        _isRunning = false;
                        break;
                    }

                    PlayerActionDto? action = _inputManager
                        .ProcessInput(key, _myPlayerId, _isViewingLog ? "ViewingLog" : _currentState);
                    
                    if (action != null)
                    {
                        if (action.ActionType == "ToggleLog" || action.ActionType == "Escape")
                        {
                            _isViewingLog = !_isViewingLog;
                            if (_lastUpdate != null)
                            {
                                _renderer.Render(_lastUpdate, _localLogs, _myPlayerId, _isViewingLog);
                            }
                            continue;
                        }
                        actionToSend = action;
                    }
                }
                if (actionToSend != null && _isRunning)
                {
                    await writer.WriteLineAsync(JsonSerializer.Serialize(actionToSend));
                }
                
                await Task.Delay(10);
            }
        }
        catch (Exception ex)
        {
            Console.Clear();
            Console.WriteLine($"Connection error: {ex.Message}");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}