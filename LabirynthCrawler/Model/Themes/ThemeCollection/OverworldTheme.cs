using LabirynthCrawler.Model.Enemies;
using LabirynthCrawler.Model.Items;
using LabirynthCrawler.Model.Items.Weapons;
using LabirynthCrawler.Model.MapGeneration;
using LabirynthCrawler.Model.Observers;

namespace LabirynthCrawler.Model.Themes.ThemeCollection;

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
    public IMapGenerationStrategy GetStrategy(){ return new OverworldGenerationStrategy(); }
    
    private readonly WeightedSpawnTable<IItem> _itemTable = new();
    private readonly WeightedSpawnTable<IWeapon> _weaponTable = new();
    private readonly WeightedSpawnTable<IEnemy> _enemyTable = new();
    private static readonly Random _rng = new Random();
    
    public OverworldFactory(ISoundPublisher soundPublisher)
    {
        // Items
        _itemTable.AddEntry(() => new Emerald(), 20);
        _itemTable.AddEntry(() => new Coin(new Random().Next(1, 11)), 50);
        _itemTable.AddEntry(() => new Gold(new Random().Next(1, 4)), 20);
        _itemTable.AddEntry(() => new GoldenApple(), 10);
        // Weapons
        _weaponTable.AddEntry(() => new WoodenAxe(), weight: 50);
        _weaponTable.AddEntry(() => new IronSword(), weight: 30);
        _weaponTable.AddEntry(() => new Bow(), weight: 20);
        // Enemies
        _enemyTable.AddEntry(() => new Zombie(new SpeciesFaction(), soundPublisher), weight: 60);
        _enemyTable.AddEntry(() => new Skeleton(new SpeciesFaction(), soundPublisher), weight: 30);
        _enemyTable.AddEntry(() => new Spider(new SpeciesFaction(), soundPublisher), weight: 10);
    }
    
    
    public string GetWelcomeMessage() => "Zapach trawy i drewna wypełnia powietrze. Słyszysz odległy śpiew...";
    
    public IItem CreateItem() => _itemTable.Spawn();
    public IWeapon CreateWeapon() => _weaponTable.Spawn();
    public IItem CreateArtifact() => new DiamondSword();
    
    
    public List<IEnemy> CreateEnemyGroup(ISoundPublisher soundPublisher)
    {
        var minNumber = 2;
        var maxNumver = 5; // exclusive
        var group = new List<IEnemy>();
        int count = new Random().Next(minNumber, maxNumver);
        for(int i = 0; i < count; i++) group.Add(_enemyTable.Spawn());
        return group;
    }
}


