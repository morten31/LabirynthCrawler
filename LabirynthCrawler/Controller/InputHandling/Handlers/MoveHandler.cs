using LabirynthCrawler.Model;
namespace LabirynthCrawler.Controller.InputHandling.Handlers;
public class MoveHandler : BaseHandler
{
    public override bool Handle(ConsoleKeyInfo key, GameModel model)
    {
        Direction? dir = null;

        if (KeyBindings.Matches(key, GameAction.MoveUp)) dir = Direction.Up;
        else if (KeyBindings.Matches(key, GameAction.MoveDown)) dir = Direction.Down;
        else if (KeyBindings.Matches(key, GameAction.MoveLeft)) dir = Direction.Left;
        else if (KeyBindings.Matches(key, GameAction.MoveRight)) dir = Direction.Right;

        if (dir != null)
        {
            model.MovePlayer(dir.Value);
            return true;
        }
        return base.Handle(key, model);
    }
}