using LabirynthCrawler.Model;
using LabirynthCrawler.Model.Network;

namespace LabirynthCrawler.Controller.InputHandling.Handlers;

public class StartEndHandler : BaseHandler
{
    public override bool Handle(PlayerActionDto action, GameModel model)
    {
        if (action.ActionType == "Quit") return false;
        if (model.CurrentState == GameModel.GameState.GameOver) return true;
        
        return base.Handle(action, model);
    }
}