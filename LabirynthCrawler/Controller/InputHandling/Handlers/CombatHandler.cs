using LabirynthCrawler.Model;
using LabirynthCrawler.Model.Network;

namespace LabirynthCrawler.Controller.InputHandling.Handlers;


public class CombatHandler : BaseHandler
{
    public override bool Handle(PlayerActionDto action, GameModel model)
    {
        if (action.ActionType == "Attack")
        {
            model.PerformAttack(action.PlayerId, action.TargetIndex);
            return true;
        }
        return base.Handle(action, model);
    }
}