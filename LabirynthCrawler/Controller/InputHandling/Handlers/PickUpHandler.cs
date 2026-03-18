using LabirynthCrawler.Model;

namespace LabirynthCrawler.Controller.InputHandling.Handlers;

public class PickUpHandler : BaseHandler
{
    public override bool Handle(ConsoleKeyInfo key, GameModel model)
    {
        if (key.Key == ConsoleKey.E)
        {
            if (model.PickUpItem())
                return true;
        }
        return base.Handle(key, model);
    }
}