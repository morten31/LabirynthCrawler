using LabirynthCrawler.Model;
using LabirynthCrawler.Model.Network;
using LabirynthCrawler.Model.Systems;

namespace LabirynthCrawler.Controller.InputHandling.Handlers;

public class PickUpHandler : BaseHandler
{
    public override bool Handle(PlayerActionDto action, GameModel model)
    {
        if (action.ActionType == "PickUp")
        {
            InteractionSystem.PickUpItem(model, action.PlayerId);
            return true;
        }
        return base.Handle(action, model);
    }
}