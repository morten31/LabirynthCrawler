using LabirynthCrawler.Model.Combat.Visitors;
using LabirynthCrawler.Model.PlayerModel;
using LabirynthCrawler.Model.Enemies;
using LabirynthCrawler.Model.Items;

namespace LabirynthCrawler.Model.Combat;

public class CombatSystem
{
    private readonly Player _player;
    private readonly IEnemy _enemy;
    private readonly CombatManager _combatManager = new CombatManager();

    public CombatSystem(Player player, IEnemy enemy)
    {
        _player = player;
       _enemy = enemy;
    }

    public CombatResult AttackEnemy(int attackType)
    {
        IAttackVisitor visitor = attackType switch
        {
            1 => new NormalAttackVisitor(_player),
            2 => new StealthAttackVisitor(_player),
            _ => new MagicAttackVisitor(_player)
        };

        return _combatManager.ResolveCombat(_player, _enemy, visitor);
    }
}