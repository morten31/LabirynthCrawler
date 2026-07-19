using LabirynthCrawler.Model;
using LabirynthCrawler.Model.Network;
using LabirynthCrawler.Model.Systems;

namespace LabirynthCrawler.Controller.InputHandling.Handlers;

public class EquipHandler : BaseHandler
{
    public override bool Handle(PlayerActionDto action, GameModel model)
    {
        if (action.ActionType == "Equip")
        {
            InteractionSystem.EquipItem(model, action.PlayerId, action.Hand, action.TargetIndex);
            return true;
        }
        return base.Handle(action, model);
    }
}