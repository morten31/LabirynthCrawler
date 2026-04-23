using LabirynthCrawler.Controller.InputHandling;
using LabirynthCrawler.Model;
using LabirynthCrawler.Model.Logger;
using LabirynthCrawler.View;


namespace LabirynthCrawler.Controller;

class Program
{
    static void Main()
    {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string projectDir = Path.GetFullPath(Path.Combine(baseDir, @"..\..\..\"));
        
        string configPath = Path.Combine(projectDir, "Controller", "config.json");
        
        GameConfig config = GameConfig.LoadConfig(configPath);
        
        GameLogger.Instance.SetWriter(new FileLogWriter(config.PlayerName, config.LogDirectory));
        GameLogger.Instance.Log($"Started game as: {config.PlayerName}");

        GameModel model = new GameModel();
        model.InitializeGame(21, 11, config.Theme);
        
        MainHandler handler = new();
        handler.Initialize();
        
        FrameRenderer renderer = new FrameRenderer();
        renderer.Initialize();
        renderer.Render(model);
        
        handler.RunGame(model, renderer);
    }
}