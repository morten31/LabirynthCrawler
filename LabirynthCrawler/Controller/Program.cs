using LabirynthCrawler.Controller.InputHandling;
using LabirynthCrawler.Controller.Network;
using LabirynthCrawler.Model;
using LabirynthCrawler.Model.Logger;
using LabirynthCrawler.View;
using LabirynthCrawler.View.Renderers;


namespace LabirynthCrawler.Controller;

class Program
{
    static async Task Main(string[] args)
    {
        bool isServer = true;
        string ip = "127.0.0.1";
        int port = 5555;
        
        if (args.Length > 0)
        {
            if (args[0] == "--client")
            {
                isServer = false;
                if (args.Length > 1)
                {
                    var parts = args[1].Split(':');
                    if (parts.Length == 2)
                    {
                        ip = parts[0];
                        int.TryParse(parts[1], out port);
                    }
                }
            }
            else if (args[0] == "--server")
            {
                isServer = true;
                if (args.Length > 1) int.TryParse(args[1], out port);
            }
        }
        else
        {
            Console.Clear();
            Console.WriteLine("Choose mode:");
            Console.WriteLine("1. (S)erver");
            Console.WriteLine("2. (C)lient");
            var key = Console.ReadKey(true).Key;
            isServer = (key != ConsoleKey.C && key != ConsoleKey.D2);
        }
        
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string projectDir = Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\"));
        string configPath = Path.Combine(projectDir, "Controller", "config.json");
        
        GameConfig config = GameConfig.LoadConfig(configPath);
        GameLogger.Instance.SetWriter(new FileLogWriter(config.PlayerName, config.LogDirectory));
        
        if (isServer)
        {
            GameLogger.Instance.Log($"Starting as SERVER on port {port}...", LogLevel.System);
            ServerHost server = new ServerHost(port, config);
            await server.StartAsync();
        }
        else
        {
            GameLogger.Instance.Log($"Starting as CLIENT, connecting to {ip}:{port}...", LogLevel.System);
            ClientHost client = new ClientHost(ip, port);
            await client.StartAsync();
        }
    }
    
    static void RunLocalOrServerMode(GameConfig config)
    {
        GameModel model = new GameModel();
        model.InitializeGame(21, 11, config.Theme);
        
        int localPlayerId = 1;

        MainHandler handler = new();
        handler.Initialize();
        
        LocalRenderer renderer = new LocalRenderer();
        renderer.Initialize();
        renderer.Render(model, localPlayerId);
    
        handler.RunGame(model, renderer, localPlayerId);
    }
}