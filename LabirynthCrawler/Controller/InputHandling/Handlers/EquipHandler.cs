using LabirynthCrawler.Model;
using LabirynthCrawler.Model.Network;

namespace LabirynthCrawler.Controller.InputHandling.Handlers;

public class EquipHandler : BaseHandler
{
    public override bool Handle(PlayerActionDto action, GameModel model)
    {
        if (action.ActionType == "Equip")
        {
            model.EquipItem(action.PlayerId, action.Hand, action.TargetIndex);
            return true;
        }
        return base.Handle(action, model);
    }
}