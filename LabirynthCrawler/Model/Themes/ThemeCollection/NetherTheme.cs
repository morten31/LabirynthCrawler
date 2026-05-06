using LabirynthCrawler.Model.Enemies;
using LabirynthCrawler.Model.Items;
using LabirynthCrawler.Model.Items.Weapons;
using LabirynthCrawler.Model.MapGeneration;
using LabirynthCrawler.Model.Observers;
using LabirynthCrawler.Model.PlayerModel;

namespace LabirynthCrawler.Model.Themes;

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

public class NetherGenerationStrategy : IMapGenerationStrategy
{
    public void Generate(IMapBuilder builder)
    {
        MapDirector director = new();
        director.BuildNetherMap(builder);
    }
}

public class NetherFactory : IThemeFactory
{
    public IMapGenerationStrategy GetStrategy() => new EndGenerationStrategy();
    
    private readonly SpeciesFaction _piglinFaction = new SpeciesFaction();
    private readonly SpeciesFaction _witherSkeletonFaction = new SpeciesFaction();
    private readonly SpeciesFaction _blazeFaction = new SpeciesFaction();
    
    private static readonly Random _rng = new Random();

    public string GetWelcomeMessage() => "Piekielne gorąco bije od ścian... Słyszysz grzechot kości Withera...";

    public List<IEnemy> CreateEnemyGroup(ISoundPublisher soundPublisher)
    {
        List<IEnemy> group = new List<IEnemy>();
        int roll = _rng.Next(3);
        int groupSize = _rng.Next(2, 5); 
        
        for (int i = 0; i < groupSize; i++)
        {
            if (roll == 0) group.Add(new Piglin(_piglinFaction, soundPublisher));
            else if (roll == 1) group.Add(new WitherSkeleton(_witherSkeletonFaction, soundPublisher));
            else group.Add(new Blaze(_blazeFaction, soundPublisher));
        }
        return group;
    }


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