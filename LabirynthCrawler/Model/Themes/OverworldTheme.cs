using LabirynthCrawler.Model.Enemies;
using LabirynthCrawler.Model.Items;
using LabirynthCrawler.Model.Items.Weapons;
using LabirynthCrawler.Model.MapGeneration;

namespace LabirynthCrawler.Model.Themes;

public class Zombie : BaseEnemy { public Zombie() : base("Zombie", health: 30, damage: 10, defence: 2) { } }
public class Skeleton : BaseEnemy { public Skeleton() : base("Skeleton", health: 20, damage: 15, defence: 0) { } }
public class Spider : BaseEnemy { public Spider() : base("Spider", health: 15, damage: 12, defence: 1) { } }

public class Emerald : QuestItem
{ 
    public Emerald() : base("Emerald") { }

}
public class GoldenApple : QuestItem
{ 
    public GoldenApple() : base("Golden Apple") { }
}

public class WoodenAxe : HeavyWeapon { public WoodenAxe() : base("Wooden Axe", 8) { } }
public class IronSword : LightWeapon { public IronSword() : base("Iron Sword", 12) { } }
public class Bow : LightWeapon { public Bow() : base("Bow", 10) { } }

public class DiamondSword : ArtifactWeapon { public DiamondSword() : base("Diamond Sword", 25) { } }

public class OverworldFactory : IThemeFactory
{
    private static readonly Random _rng = new Random();

    public string GetWelcomeMessage() => "Zapach trawy i drewna wypełnia powietrze. Słyszysz muzykę...";

    public IEnemy CreateEnemy() => _rng.Next(3) switch
    {
        0 => new Zombie(),
        1 => new Skeleton(),
        _ => new Spider()
    };

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

public class OverworldGenerationStrategy : IMapGenerationStrategy
{
    public void Generate(IMapBuilder builder)
    {
        builder
            .BuildEmpty()
            .FillWithWalls()
            .AddMainRoom(14, 10) 
            .AddRooms(3, 4)
            .AddCorridors()
            .AddItems(8)
            .AddWeapons(4)
            .AddEnemies(6)
            .AddArtifact();
    }
}