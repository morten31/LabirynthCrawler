using LabirynthCrawler.Model.Logger;

namespace LabirynthCrawler.Model.Systems;

public static class MovementSystem
{
    public static void MovePlayer(GameModel model, int playerId, Direction dir)
    {
        var p = model.GetPlayer(playerId);
        if (p == null || p.IsDead) return;

        int oldX = p.GetX();
        int oldY = p.GetY();

        (int newX, int newY) = dir switch
        {
            Direction.Up => (oldX, oldY - 1),
            Direction.Down => (oldX, oldY + 1),
            Direction.Left => (oldX - 1, oldY),
            Direction.Right => (oldX + 1, oldY),
            _ => (oldX, oldY)
        };

        if (model.GetMap().IsWithinBounds(newX, newY))
        {
            if (model.GetMap().GetTile(newX, newY).IsWall())
            {
                GameLogger.Instance.Log("Failed! A wall is on the way!", LogLevel.Trace, playerId);
            }
            else
            {
                p.MoveTo(newX, newY);
            }
        }
    }
}