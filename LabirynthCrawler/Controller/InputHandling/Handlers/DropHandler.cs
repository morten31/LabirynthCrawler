using LabirynthCrawler.Model;
using LabirynthCrawler.Model.Network;
using LabirynthCrawler.Model.Systems;

namespace LabirynthCrawler.Controller.InputHandling.Handlers;

public class DropHandler : BaseHandler
{
    public override bool Handle(PlayerActionDto action, GameModel model)
    {
        if (action.ActionType == "Drop")
        {
            InteractionSystem.DropItem(model, action.PlayerId, action.TargetIndex);
            return true;
        }
        return base.Handle(action, model);
    }
}