using LabirynthCrawler.Model.Enemies;
using LabirynthCrawler.Model.Items;
using LabirynthCrawler.Model.Items.Weapons;
using LabirynthCrawler.Model.MapGeneration;
using LabirynthCrawler.Model.PlayerModel;

namespace LabirynthCrawler.Model.Themes;

public class Piglin : BaseEnemy { public Piglin() : base("Piglin", health: 35, damage: 18, defence: 3) { } }
public class WitherSkeleton : BaseEnemy { public WitherSkeleton() : base("Wither Skeleton", health: 25, damage: 25, defence: 1) { } }
public class Blaze : BaseEnemy { public Blaze() : base("Blaze", health: 40, damage: 15, defence: 5) { } }


public class GoldNugget : QuestItem
{ 
    public GoldNugget() : base("Gold Nugget") { }

}
public class NetherQuartz : QuestItem
{ 
    public NetherQuartz() : base("Nether Quartz") { }

}

public class GoldenSword : LightWeapon { public GoldenSword() : base("Golden Sword", 15) { } }
public class NetheriteAxe : HeavyWeapon { public NetheriteAxe() : base("Netherite Axe", 22) { } }
public class BlazeRod : MagicWeapon { public BlazeRod() : base("Blaze Rod", 18) { } }



public class WitherScythe : ArtifactWeapon 
{ 
    public WitherScythe() : base("Wither Scythe", 45) { } 

    public override Attributes GetAttributes() => new Attributes(power: 15, aggression: 20, health: -20, luck: -10);
}


public class NetherFactory : IThemeFactory
{
    private static readonly Random _rng = new Random();

    public string GetWelcomeMessage() => "Piekielne gorąco bije od ścian... Słyszysz grzechot kości Withera...";

    public IEnemy CreateEnemy() => _rng.Next(3) switch
    {
        0 => new Piglin(),
        1 => new WitherSkeleton(),
        _ => new Blaze()
    };

    public IItem CreateItem() => _rng.Next(4) switch
    {
        0 => new GoldNugget(),
        1 => new Coin(_rng.Next(15)),
        2 => new Gold(_rng.Next(5)),
        _ => new NetherQuartz()
        
    };

    public IWeapon CreateWeapon() => _rng.Next(3) switch
    {
        0 => new GoldenSword(),
        1 => new NetheriteAxe(),
        _ => new BlazeRod()
    };

    public IItem CreateArtifact() => new WitherScythe();
}

public class NetherGenerationStrategy : IMapGenerationStrategy
{
    public void Generate(IMapBuilder builder)
    {
        builder
            .BuildEmpty()
            .FillWithWalls()
            .AddCorridors()
            .AddCorridors()
            .AddRooms(6, 3)
            .AddItems(5)
            .AddWeapons(4)
            .AddEnemies(9)
            .AddArtifact(); 
    }
}