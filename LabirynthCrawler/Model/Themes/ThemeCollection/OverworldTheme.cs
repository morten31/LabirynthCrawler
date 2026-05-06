using LabirynthCrawler.Model.Enemies;
using LabirynthCrawler.Model.Items;
using LabirynthCrawler.Model.Items.Weapons;
using LabirynthCrawler.Model.MapGeneration;
using LabirynthCrawler.Model.Observers;

namespace LabirynthCrawler.Model.Themes;

public class Emerald : QuestItem { public Emerald() : base("Emerald") { } }
public class GoldenApple : QuestItem { public GoldenApple() : base("Golden Apple") { } }
public class WoodenAxe : HeavyWeapon { public WoodenAxe() : base("Wooden Axe", 8) { } }
public class IronSword : LightWeapon { public IronSword() : base("Iron Sword", 12) { } }
public class Bow : LightWeapon { public Bow() : base("Bow", 10) { } }
public class DiamondSword : ArtifactWeapon { public DiamondSword() : base("Diamond Sword", 25) { } }

public class OverworldGenerationStrategy : IMapGenerationStrategy
{
    public void Generate(IMapBuilder builder)
    {
        MapDirector director = new();
        director.BuildOverworldMap(builder);
    }
}

public class OverworldFactory : IThemeFactory
{
    public IMapGenerationStrategy GetStrategy(){
        return new OverworldGenerationStrategy();
    }
    
    private readonly SpeciesFaction _zombieFaction = new SpeciesFaction();
    private readonly SpeciesFaction _skeletonFaction = new SpeciesFaction();
    private readonly SpeciesFaction _spiderFaction = new SpeciesFaction();
    
    private static readonly Random _rng = new Random();
    
    public string GetWelcomeMessage() => "Zapach trawy i drewna wypełnia powietrze. Słyszysz muzykę...";
    
    public List<IEnemy> CreateEnemyGroup(ISoundPublisher soundPublisher)
    {
        List<IEnemy> group = new List<IEnemy>();
        int roll = _rng.Next(3);
        int groupSize = _rng.Next(2, 5);
        
        for (int i = 0; i < groupSize; i++)
        {
            if (roll == 0) group.Add(new Zombie(_zombieFaction, soundPublisher));
            else if (roll == 1) group.Add(new Skeleton(_skeletonFaction, soundPublisher));
            else group.Add(new Spider(_spiderFaction, soundPublisher));
        }
        return group;
    }
    

    public IItem CreateItem() => _rng.Next(4) switch
    {
        0 => new Emerald(),
        1 => new Coin(_rng.Next(10)),
        2 => new Gold(_rng.Next(3)),
        _ => new GoldenApple()
    };

    public IWeapon CreateWeapon() => _rng.Next(3) switch
    {
        0 => new WoodenAxe(),
        1 => new IronSword(),
        _ => new Bow()
    };

    public IItem CreateArtifact() => new DiamondSword();
}


