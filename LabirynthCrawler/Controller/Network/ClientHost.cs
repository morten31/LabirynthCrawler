using System.Net.Sockets;
using System.Text.Json;
using LabirynthCrawler.Controller.InputHandling;
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
    private volatile string _currentState = "Playing";

    public ClientHost(string ip, int port)
    {
        _ip = ip;
        _port = port;
        _renderer = new ClientRenderer();
    }

    public void Start()
    {
        Console.Clear();
        Console.WriteLine($"Connecting to server {_ip}:{_port}...");

        try
        {
            using TcpClient client = new TcpClient(_ip, _port);
            using NetworkStream stream = client.GetStream();
            using StreamReader reader = new StreamReader(stream);
            using StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

            string? welcomeJson = reader.ReadLine();
            if (welcomeJson != null)
            {
                var welcome = JsonSerializer.Deserialize<WelcomeDto>(welcomeJson);
                if (welcome != null)
                {
                    _myPlayerId = welcome.AssignedPlayerId;
                    Console.WriteLine($"Connected! you are player {_myPlayerId}.");
                    Thread.Sleep(1000);
                 
                    _renderer.Initialize(welcome);
                }
            }


            Task.Run(() =>
            {
                try
                {
                    while (_isRunning)
                    {
                        string? stateJson = reader.ReadLine();
                        if (stateJson == null) break;

                        var gameState = JsonSerializer.Deserialize<GameStateDto>(stateJson);
                        if (gameState != null)
                        {
                            _currentState = gameState.CurrentState;
                            _renderer.Render(gameState, _myPlayerId);
                        }
                    }
                }
                catch (Exception) { /* Server closed*/ }
                _isRunning = false;
            });

            while (_isRunning)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    PlayerActionDto? action = null;

                    if (_currentState == "ViewingLog" && (key.Key == ConsoleKey.Escape || KeyBindings.Matches(key, GameAction.ToggleLog)))
                    {
                        action = new PlayerActionDto { PlayerId = _myPlayerId, ActionType = "Escape" };

                    }
                    else if (KeyBindings.Matches(key, GameAction.Quit))
                    {
                        _isRunning = false;
                        break;
                    }
                    else
                    {
                        action = ParseKeyToAction(key);
                    }
                    
                    if (action != null)
                    {
                        writer.WriteLine(JsonSerializer.Serialize(action));
                    }
                }
                Thread.Sleep(10);
            }
        }
        catch (Exception ex)
        {
            Console.Clear();
            Console.WriteLine($"Błąd połączenia: {ex.Message}");
            Console.WriteLine("Naciśnij dowolny przycisk, aby wyjść.");
            Console.ReadKey();
        }
    }

    private PlayerActionDto? ParseKeyToAction(ConsoleKeyInfo key)
    {
        var action = new PlayerActionDto { PlayerId = _myPlayerId };

        if (KeyBindings.Matches(key, GameAction.MoveUp)) { action.ActionType = "Move"; action.Direction = "Up"; return action; }
        if (KeyBindings.Matches(key, GameAction.MoveDown)) { action.ActionType = "Move"; action.Direction = "Down"; return action; }
        if (KeyBindings.Matches(key, GameAction.MoveLeft)) { action.ActionType = "Move"; action.Direction = "Left"; return action; }
        if (KeyBindings.Matches(key, GameAction.MoveRight)) { action.ActionType = "Move"; action.Direction = "Right"; return action; }

        if (KeyBindings.Matches(key, GameAction.PickUp)) { action.ActionType = "PickUp"; return action; }

        if (KeyBindings.Matches(key, GameAction.Drop))
        {
            var k2 = Console.ReadKey(true);
            if (char.IsDigit(k2.KeyChar))
            {
                action.ActionType = "Drop";
                action.TargetIndex = int.Parse(k2.KeyChar.ToString());
                return action;
            }
        }

        bool isLeft = KeyBindings.Matches(key, GameAction.EquipLeft);
        bool isRight = KeyBindings.Matches(key, GameAction.EquipRight);
        if (isLeft || isRight)
        {
            var k2 = Console.ReadKey(true);
            if (char.IsDigit(k2.KeyChar))
            {
                action.ActionType = "Equip";
                action.Hand = isLeft ? 'L' : 'R';
                action.TargetIndex = int.Parse(k2.KeyChar.ToString());
                return action;
            }
        }

        if (KeyBindings.Matches(key, GameAction.AttackNormal)) { action.ActionType = "Attack"; action.TargetIndex = 1; return action; }
        if (KeyBindings.Matches(key, GameAction.AttackStealth)) { action.ActionType = "Attack"; action.TargetIndex = 2; return action; }
        if (KeyBindings.Matches(key, GameAction.AttackMagic)) { action.ActionType = "Attack"; action.TargetIndex = 3; return action; }

        return null;
    }
}