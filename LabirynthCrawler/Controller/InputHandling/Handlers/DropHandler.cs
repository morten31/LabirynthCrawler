using LabirynthCrawler.Model;
using LabirynthCrawler.Model.Network;

namespace LabirynthCrawler.Controller.InputHandling.Handlers;

public class DropHandler : BaseHandler
{
    public override bool Handle(PlayerActionDto action, GameModel model)
    {
        if (action.ActionType == "Drop")
        {
            model.DropItem(action.PlayerId, action.TargetIndex);
            return true;
        }
        return base.Handle(action, model);
    }
}