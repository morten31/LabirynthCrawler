using LabirynthCrawler.Model;
using LabirynthCrawler.Model.Network;

namespace LabirynthCrawler.Controller.InputHandling.Handlers;

public class LogViewHandler : BaseHandler
{
    public override bool Handle(PlayerActionDto action, GameModel model)
    {
        if (action.ActionType == "ToggleLog" || action.ActionType == "Escape")
        {
            var player = model.GetPlayer(action.PlayerId);
            if (player != null)
            {
                player.IsViewingLog = !player.IsViewingLog;
            }
            return true;
        }
        return base.Handle(action, model);
    }
}