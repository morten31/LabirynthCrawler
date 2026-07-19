using LabirynthCrawler.Model;
using LabirynthCrawler.Model.Network;
using LabirynthCrawler.Model.Systems;

namespace LabirynthCrawler.Controller.InputHandling.Handlers;
public class MoveHandler : BaseHandler
{
    public override bool Handle(PlayerActionDto action, GameModel model)
    {
        if (action.ActionType == "Move")
        {
            if (Enum.TryParse<Direction>(action.Direction, out var dir))
                MovementSystem.MovePlayer(model, action.PlayerId, dir);
            return true;
        }
        return base.Handle(action, model);
    }
}