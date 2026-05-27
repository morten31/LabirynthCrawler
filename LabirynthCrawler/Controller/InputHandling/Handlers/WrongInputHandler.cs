
using LabirynthCrawler.Model;
using LabirynthCrawler.Model.Logger;
using LabirynthCrawler.Model.Network;

namespace LabirynthCrawler.Controller.InputHandling.Handlers;

public class WrongInputHandler : BaseHandler
{
    public override bool Handle(PlayerActionDto action, GameModel model)
    {
        if (action.ActionType == "WrongInput")
        {
            GameLogger.Instance.Log($"Unknown key: {action.Direction}", LogLevel.Trace, action.PlayerId);
            return true;
        }
        return base.Handle(action, model);
    }
}