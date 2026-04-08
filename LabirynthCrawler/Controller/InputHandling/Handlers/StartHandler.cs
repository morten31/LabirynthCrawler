using LabirynthCrawler.Model;

namespace LabirynthCrawler.Controller.InputHandling.Handlers;

public class StartHandler : BaseHandler
{
    public override bool Handle(ConsoleKeyInfo key, GameModel model)
    {
        if (KeyBindings.Matches(key, GameAction.Quit))
        {
            return false;
        }
        return base.Handle(key, model);
    }
}