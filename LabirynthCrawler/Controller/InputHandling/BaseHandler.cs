using LabirynthCrawler.Model;
using LabirynthCrawler.Model.Network;

namespace LabirynthCrawler.Controller.InputHandling;

public abstract class BaseHandler
{
    private BaseHandler? _next;

    public void SetNext(BaseHandler next)
    {
        _next = next;
    }

    public virtual bool Handle(PlayerActionDto action, GameModel model)
    {
        var player = model.GetPlayer(action.PlayerId);
        if (player != null && player.IsDead)
        {
            if (action.ActionType != "Quit" && action.ActionType != "ToggleLog" && action.ActionType != "Escape")
            {
                return true;
            }
        }

        if (_next != null)
            return _next.Handle(action, model);
        
        return true;
    }
}