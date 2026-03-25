using LabirynthCrawler.Controller.InputHandling;
using LabirynthCrawler.Model;
using LabirynthCrawler.View;


namespace LabirynthCrawler.Controller;

class Program
{
    static void Main()
    {
        GameModel model = new GameModel();
        model.InitializeGame(21, 11);
        
        MainHandler handler = new();
        handler.Initialize();
        
        FrameRenderer renderer = new FrameRenderer();
        renderer.Initialize();
        renderer.Render(model);
        
        handler.RunGame(model, renderer);
    }
}