using LabirynthCrawler.Model;
using LabirynthCrawler.Model.Network;

namespace LabirynthCrawler.Controller.InputHandling.Handlers;

public class PickUpHandler : BaseHandler
{
    public override bool Handle(PlayerActionDto action, GameModel model)
    {
        if (action.ActionType == "PickUp")
        {
            model.PickUpItem(action.PlayerId);
            return true;
        }
        return base.Handle(action, model);
    }
}