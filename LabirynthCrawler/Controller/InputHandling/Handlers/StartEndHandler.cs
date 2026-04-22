using LabirynthCrawler.Model;

namespace LabirynthCrawler.Controller.InputHandling.Handlers;

public class StartEndHandler : BaseHandler
{
    public override bool Handle(ConsoleKeyInfo key, GameModel model)
    {
        if (KeyBindings.Matches(key, GameAction.Quit))
        {
            return false;
        }

        if (model.CurrentState == GameModel.GameState.GameOver) return true;
        
        return base.Handle(key, model);
    }
}