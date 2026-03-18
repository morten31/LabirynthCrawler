using LabirynthCrawler.Model;

namespace LabirynthCrawler.Controller.InputHandling;

public abstract class BaseHandler
{
    private BaseHandler? _next;

    public void SetNext(BaseHandler next)
    {
        _next = next;
    }

    public virtual bool Handle(ConsoleKeyInfo key, GameModel model)
    {
        if (_next != null)
        {
            return _next.Handle(key, model);
        }

        return true;
    }
}