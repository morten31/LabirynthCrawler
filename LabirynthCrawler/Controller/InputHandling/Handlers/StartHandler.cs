using LabirynthCrawler.Model;

namespace LabirynthCrawler.Controller.InputHandling.Handlers;

public class StartHandler : BaseHandler
{
    public override bool Handle(ConsoleKeyInfo key, GameModel model)
    {
        if (key.Key == ConsoleKey.Escape)
        {
            return false;
        }
        return base.Handle(key, model);
    }
}