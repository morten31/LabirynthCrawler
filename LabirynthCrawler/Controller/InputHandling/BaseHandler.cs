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
        if (_next != null)
            return _next.Handle(action, model);
        
        return true;
    }
}