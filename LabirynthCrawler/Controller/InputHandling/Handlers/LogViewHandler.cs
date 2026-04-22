using LabirynthCrawler.Model;

namespace LabirynthCrawler.Controller.InputHandling.Handlers;

public class LogViewHandler : BaseHandler
{
    public override bool Handle(ConsoleKeyInfo key, GameModel model)
    {
        if (KeyBindings.Matches(key, GameAction.ToggleLog))
        {
            if (model.CurrentState == GameModel.GameState.Playing)
                model.CurrentState = GameModel.GameState.ViewingLog;
            else if (model.CurrentState == GameModel.GameState.ViewingLog)
                model.CurrentState = GameModel.GameState.Playing;
            
            return true;
        }

        if (model.CurrentState == GameModel.GameState.ViewingLog)
        {
            if (key.Key == ConsoleKey.Escape) model.CurrentState = GameModel.GameState.Playing;
            return true; 
        }

        return base.Handle(key, model);
    }
}