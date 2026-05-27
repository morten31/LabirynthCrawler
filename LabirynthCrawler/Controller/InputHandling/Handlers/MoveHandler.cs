using LabirynthCrawler.Model;
using LabirynthCrawler.Model.Network;

namespace LabirynthCrawler.Controller.InputHandling.Handlers;
public class MoveHandler : BaseHandler
{
    public override bool Handle(PlayerActionDto action, GameModel model)
    {
        if (action.ActionType == "Move")
        {
            if (Enum.TryParse<Direction>(action.Direction, out var dir))
                model.MovePlayer(action.PlayerId, dir);
            return true;
        }
        return base.Handle(action, model);
    }
}