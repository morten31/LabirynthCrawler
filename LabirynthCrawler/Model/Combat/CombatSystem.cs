using LabirynthCrawler.Model.Combat.Visitors;
using LabirynthCrawler.Model.PlayerModel;
using LabirynthCrawler.Model.Enemies;
using LabirynthCrawler.Model.Items;

namespace LabirynthCrawler.Model.Combat;

public class CombatSystem
{
    public bool ExecuteTurn(Player player, IEnemy enemy, IAttackVisitor attackVisitor)
    {
        var activeItems = player.GetInventory().GetEquippedItems();
        
        int playerTotalDamage = 0;
        int playerTotalDefense = 0;

        if (activeItems.Count == 0)
        {
            // Atak gołymi rękami
            var (dmg, def) = attackVisitor.Visit((IItem)null!);
            playerTotalDamage = dmg;
            playerTotalDefense = def;
        }
        else
        {
            foreach (var item in activeItems)
            {
                var stats = item.Accept(attackVisitor);
                playerTotalDamage += stats.Damage;
                playerTotalDefense = Math.Max(playerTotalDefense, stats.Defense);
            }
        }

        // Faza 1: Gracz zadaje obrażenia
        enemy.DecreaseHealth(playerTotalDamage);

        // Faza 2: Przeciwnik oddaje (jeśli żyje)
        if (enemy.GetHealth() > 0)
        {
            int enemyDamage = Math.Max(0, enemy.GetDamage() - playerTotalDefense);
            player.GetAttributes().Health -= enemyDamage;
        }

        return player.GetAttributes().Health <= 0;
    }
}