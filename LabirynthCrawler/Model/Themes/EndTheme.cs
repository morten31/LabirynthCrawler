using LabirynthCrawler.Model.Enemies;
using LabirynthCrawler.Model.Items;
using LabirynthCrawler.Model.Items.Weapons;
using LabirynthCrawler.Model.MapGeneration;
using LabirynthCrawler.Model.PlayerModel;

namespace LabirynthCrawler.Model.Themes;


public class Enderman : BaseEnemy { public Enderman() : base("Enderman", health: 60, damage: 20, defence: 2) { } }
public class Shulker : BaseEnemy { public Shulker() : base("Shulker", health: 25, damage: 10, defence: 15) { } }
public class EnderDragon : BaseEnemy { public EnderDragon() : base("Ender Dragon", health: 200, damage: 20, defence: 10) { } }


public class EnderPearl : QuestItem
{
    public EnderPearl() : base("Ender Pearl") { }
}

public class ChorusFruit : QuestItem
{
    public ChorusFruit() : base("Chorus Fruit") { }
}

public class VoidSword : MagicWeapon { public VoidSword() : base("Void Sword", 25) { } }
public class ObsidianMace : HeavyWeapon { public ObsidianMace() : base("Obsidian Mace", 30) { } }
public class ShulkerShield : LightWeapon { public ShulkerShield() : base("Shulker Shield", 5) { } }


public class DragonSlayerHalberd : ArtifactWeapon 
{ 
    public DragonSlayerHalberd() : base("Dragon Slayer Halberd", 50) { } 

    public override Attributes GetAttributes() => new(power: 20, wisdom: 25, luck: 5);
}


public class EndFactory : IThemeFactory
{
    private static readonly Random _rng = new Random();

    public string GetWelcomeMessage() => "Otacza Cię bezkresna pustka. Patrz pod nogi i... nie patrz Czarnym w oczy.";

    public IEnemy CreateEnemy() => _rng.Next(10) switch
    {
        < 5 => new Enderman(), // 50%
        < 8 => new Shulker(),  // 30%
        _ => new EnderDragon()   // 20%
    };

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


public class EndGenerationStrategy : IMapGenerationStrategy
{
    public void Generate(IMapBuilder builder)
    {
        builder
            .BuildEmpty()
            .FillWithWalls()
            .AddMainRoom(20, 10)
            .AddRooms(12, 2)
            .AddItems(4)
            .AddWeapons(3)
            .AddEnemies(8)
            .AddArtifact(); 
    }
}