using LabirynthCrawler.Model;
using LabirynthCrawler.Model.Combat;
using LabirynthCrawler.Model.Network;

namespace LabirynthCrawler.Controller.InputHandling.Handlers;


public class CombatHandler : BaseHandler
{
    public override bool Handle(PlayerActionDto action, GameModel model)
    {
        if (action.ActionType == "Attack")
        {
            CombatService.PerformAttack(model, action.PlayerId, action.TargetIndex);
            return true;
        }
        return base.Handle(action, model);
    }
}