using LabirynthCrawler.Model.Board;
using LabirynthCrawler.Model.Enemies;
using LabirynthCrawler.Model.Logger;

namespace LabirynthCrawler.Model.Combat;

public static class CombatService
{
    public static void PerformAttack(GameModel model, int playerId, int attackType)
    {
        var p = model.GetPlayer(playerId);
        if (p == null || p.IsDead) return;

        Tile currentTile = model.GetMap().GetTile(p.GetX(), p.GetY());
        var enemies = currentTile.GetEnemies();

        if (enemies.Count == 0)
        {
            GameLogger.Instance.Log("There are no enemies to attack!", LogLevel.Trace, playerId);
            return;
        }

        IEnemy enemy = enemies[0];
        CombatModel combat = new CombatModel(p, enemy);
        CombatResult result = combat.AttackEnemy(attackType);

        GameLogger.Instance.Log($"You dealt {result.DamageDealt} dmg to {enemy.ToString()}.", LogLevel.Combat, playerId);

        if (result.IsEnemyDead)
        {
            GameLogger.Instance.Log($"{enemy.ToString()} was killed!", LogLevel.Combat, playerId);
            currentTile.RemoveDeadEnemies();
        }
        else if (!result.IsPlayerDead)
        {
            GameLogger.Instance.Log($"{enemy.ToString()} hit back for {result.DamageReceived} dmg.", LogLevel.Combat, playerId);
        }

        if (result.IsPlayerDead)
        {
            GameLogger.Instance.Log($"Player {playerId} was killed by {enemy.ToString()}!", LogLevel.System);
            GameLogger.Instance.Log("YOU DIED! Spectating mode.", LogLevel.System, playerId);

            p.IsDead = true;

            if (model.Players.Values.All(player => player.IsDead))
            {
                GameLogger.Instance.Log("All players dead! Ending Game...", LogLevel.System);
                model.CurrentState = GameModel.GameState.GameOver;
            }
        }
    }
}