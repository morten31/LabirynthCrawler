namespace LabirynthCrawler.Model.Systems;

public class EnemyAISystem
{
    private DateTime _lastEnemyMoveTime = DateTime.Now;

    public void Tick(GameModel model)
    {
        if ((DateTime.Now - _lastEnemyMoveTime).TotalMilliseconds >= 1000)
        {
            var allEnemies = model.GetMap().GetAllEnemies().ToList();
            foreach (var enemy in allEnemies)
            {
                enemy.MoveRandomly(model.GetMap());
            }
            _lastEnemyMoveTime = DateTime.Now;
        }
    }
}