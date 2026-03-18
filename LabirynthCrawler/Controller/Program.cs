using LabirynthCrawler.Controller.InputHandling;
using LabirynthCrawler.Model;
using LabirynthCrawler.View;


namespace LabirynthCrawler.Controller;

class Program
{
    static void Main()
    {
        GameModel model = new GameModel();
        MainHandler handler = new();
        handler.Initialize(model);
        
        FrameRenderer renderer = new FrameRenderer();
        renderer.Initialize();
        renderer.Render(model);
        
        handler.RunGame(model, renderer);
    }
}