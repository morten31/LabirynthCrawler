using LabirynthCrawler.Model;
using LabirynthCrawler.Model.Network;

namespace LabirynthCrawler.Controller.InputHandling.Handlers;

public class LogViewHandler : BaseHandler
{
    public override bool Handle(PlayerActionDto action, GameModel model)
    {
        var player = model.GetPlayer(action.PlayerId);
        if (player == null) return base.Handle(action, model);

        if (action.ActionType == "ToggleLog")
        {
            player.IsViewingLog = !player.IsViewingLog;
            return true;
        }

        if (action.ActionType == "Escape" && player.IsViewingLog)
        {
            player.IsViewingLog = false;
            return true;
        }

        return base.Handle(action, model);
    }
}