using LabirynthCrawler.Model;
namespace LabirynthCrawler.Controller.InputHandling.Handlers;
public class MoveHandler : BaseHandler
{
    public override bool Handle(ConsoleKeyInfo key, GameModel model)
    {
        Direction? dir = key.Key switch
        {
            ConsoleKey.W => Direction.Up,
            ConsoleKey.A => Direction.Left,
            ConsoleKey.S => Direction.Down,
            ConsoleKey.D => Direction.Right,
            _ => null
        };

        if (dir != null)
        {
            model.MovePlayer(dir.Value);
            return true;
        }
        return base.Handle(key, model);
    }
}