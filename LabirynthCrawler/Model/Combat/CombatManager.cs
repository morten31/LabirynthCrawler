using LabirynthCrawler.Model.Combat.Visitors;
using LabirynthCrawler.Model.PlayerModel;
using LabirynthCrawler.Model.Enemies;
using LabirynthCrawler.Model.Items;

namespace LabirynthCrawler.Model.Combat;

public class CombatManager
{
    public (int totalDamage, int damageReceived) GetFightStats(Player player, IEnemy enemy, IAttackVisitor visitor)
    {
        int totalDamage = 0;
        int totalDefence = 0;

        List<IItem> itemsInHands = player.GetInventory().GetEquippedItems();

        if (itemsInHands.Count == 0)
        {
            var stats = visitor.Visit((IItem)null!);
            totalDamage += stats.Damage;
            totalDefence += stats.Defense;
        }
        else
        {
            foreach (var item in itemsInHands)
            {
                var stats = item.Accept(visitor);
                totalDamage += stats.Damage;
                totalDefence += stats.Defense;
            }
        }

        totalDamage -= enemy.GetDefence();
        totalDamage = Math.Max(totalDamage, 0);

        int rawReceived = enemy.GetDamage() - totalDefence;
        int damageReceived = Math.Max(rawReceived, 0);

        return (totalDamage, damageReceived);
    }

    public CombatResult ResolveCombat(Player player, IEnemy enemy, IAttackVisitor visitor)
    {
        var (damageToEnemy, damageToPlayer) = GetFightStats(player, enemy, visitor);
        
        bool isEnemyDead = enemy.GetHealth() - damageToEnemy <= 0;

        CombatResult result = new CombatResult
        {
            DamageDealt = damageToEnemy,
            DamageReceived = isEnemyDead == false ? damageToPlayer : 0,
            IsEnemyDead = isEnemyDead,
            IsPlayerDead = player.GetTotalAttributes().Health - damageToPlayer <= 0
        };

        enemy.DecreaseHealth(result.DamageDealt);
        if (!result.IsEnemyDead)
        {
            player.GetAttributes().Health -= result.DamageReceived;
        }

        return result;
    }
}
public class CombatResult
{
    public int DamageDealt { get; set; }
    public int DamageReceived { get; set; }
    public bool IsEnemyDead { get; set; }
    public bool IsPlayerDead { get; set; }
}