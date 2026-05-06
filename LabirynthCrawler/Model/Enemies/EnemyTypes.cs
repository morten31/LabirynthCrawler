using LabirynthCrawler.Model.Logger;
using LabirynthCrawler.Model.Observers;

namespace LabirynthCrawler.Model.Enemies;

public class Goblin : BaseEnemy
{
    public Goblin(ISpeciesPublisher faction, ISoundPublisher sound)
        : base("Goblin", health: 20, damage: 8, defence: 1, faction, sound) { }
}

public class Orc : BaseEnemy
{
    public Orc(ISpeciesPublisher faction, ISoundPublisher sound)
        : base("Orc", health: 45, damage: 15, defence: 4, faction, sound) { }
}

public class Troll : BaseEnemy
{
    public Troll(ISpeciesPublisher faction, ISoundPublisher sound) 
        : base("Troll", health: 80, damage: 25, defence: 10, faction, sound) { }
}

public class Zombie : BaseEnemy
{
    public Zombie(ISpeciesPublisher faction, ISoundPublisher sound) 
        : base("Zombie", health: 30, damage: 10, defence: 2, faction, sound) { }
    
    public override void ReactToAllyDeath()
    {
        _damage += 2;
    }
}
public class Skeleton : BaseEnemy {
    public Skeleton(ISpeciesPublisher faction, ISoundPublisher sound)
        : base("Skeleton", health: 20, damage: 15, defence: 0, faction, sound)
    { }

    public override void ReactToAllyDeath()
    {
    }
}

public class Spider : BaseEnemy
{
    public Spider(ISpeciesPublisher faction, ISoundPublisher sound) 
        : base("Spider", health: 15, damage: 12, defence: 1, faction, sound) { }
    public override void ReactToAllyDeath()
    {
        _defence = Math.Max(0, _defence - 1); 
    }
}   

public class Piglin : BaseEnemy { public Piglin(ISpeciesPublisher faction, ISoundPublisher sound) 
    : base("Piglin", health: 35, damage: 18, defence: 3, faction, sound) { } }
public class WitherSkeleton : BaseEnemy { public WitherSkeleton(ISpeciesPublisher faction, ISoundPublisher sound)
    : base("Wither Skeleton", health: 25, damage: 25, defence: 1, faction, sound) { } }
public class Blaze : BaseEnemy { public Blaze(ISpeciesPublisher faction, ISoundPublisher sound)
    : base("Blaze", health: 40, damage: 15, defence: 5, faction, sound) { } }

public class Enderman : BaseEnemy { public Enderman(ISpeciesPublisher faction, ISoundPublisher sound)
    : base("Enderman", health: 60, damage: 20, defence: 2, faction, sound) { } }
public class Shulker : BaseEnemy { public Shulker(ISpeciesPublisher faction, ISoundPublisher sound)
    : base("Shulker", health: 25, damage: 10, defence: 15, faction, sound) { } }
public class EnderDragon : BaseEnemy { public EnderDragon(ISpeciesPublisher faction, ISoundPublisher sound)
    : base("Ender Dragon", health: 200, damage: 20, defence: 10, faction, sound) { } }
