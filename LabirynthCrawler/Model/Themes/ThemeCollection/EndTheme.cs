using LabirynthCrawler.Model.Enemies;
using LabirynthCrawler.Model.Items;
using LabirynthCrawler.Model.Items.Weapons;
using LabirynthCrawler.Model.MapGeneration;
using LabirynthCrawler.Model.Observers;
using LabirynthCrawler.Model.PlayerModel;

namespace LabirynthCrawler.Model.Themes;

public class EnderPearl : QuestItem { public EnderPearl() : base("Ender Pearl") { } }
public class ChorusFruit : QuestItem { public ChorusFruit() : base("Chorus Fruit") { } }
public class VoidSword : MagicWeapon { public VoidSword() : base("Void Sword", 25) { } }
public class ObsidianMace : HeavyWeapon { public ObsidianMace() : base("Obsidian Mace", 30) { } }
public class ShulkerShield : LightWeapon { public ShulkerShield() : base("Shulker Shield", 5) { } }

public class DragonSlayerHalberd : ArtifactWeapon 
{ 
    public DragonSlayerHalberd() : base("Dragon Slayer Halberd", 50) { } 

    public override Attributes GetAttributes() => new(power: 20, wisdom: 25, luck: 5);
}


public class EndGenerationStrategy : IMapGenerationStrategy
{
    public void Generate(IMapBuilder builder)
    {
        MapDirector director = new();
        director.BuildEndMap(builder);
    }
}

public class EndFactory : IThemeFactory
{
    public IMapGenerationStrategy GetStrategy(){
        return new EndGenerationStrategy();
    }
    
    private readonly SpeciesFaction _endermanFaction = new SpeciesFaction();
    private readonly SpeciesFaction _shulkerFaction = new SpeciesFaction();
    private readonly SpeciesFaction _enderDragonFaction = new SpeciesFaction();
    
    private static readonly Random _rng = new Random();

    public string GetWelcomeMessage() => "Otacza Cię bezkresna pustka. Patrz pod nogi i... nie patrz Czarnym w oczy.";

    public List<IEnemy> CreateEnemyGroup(ISoundPublisher soundPublisher)
    {
        List<IEnemy> group = new List<IEnemy>();
        int roll = _rng.Next(10);
        
        int groupSize = roll >= 8 ? 2 : _rng.Next(2, 5); 
        
        for (int i = 0; i < groupSize; i++)
        {
            if (roll < 5) group.Add(new Enderman(_endermanFaction, soundPublisher)); // 50%
            else if (roll < 8) group.Add(new Shulker(_shulkerFaction, soundPublisher)); // 30%
            else group.Add(new EnderDragon(_enderDragonFaction, soundPublisher)); // 20%
        }
        return group;
    }

    public IItem CreateItem() => _rng.Next(2) switch
    {
        0 => new EnderPearl(),
        _ => new ChorusFruit()
    };

    public IWeapon CreateWeapon() => _rng.Next(3) switch
    {
        0 => new VoidSword(),
        1 => new ObsidianMace(),
        _ => new ShulkerShield()
    };

    public IItem CreateArtifact() => new DragonSlayerHalberd();
}
