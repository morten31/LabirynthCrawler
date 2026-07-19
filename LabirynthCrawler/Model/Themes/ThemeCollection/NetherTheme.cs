using LabirynthCrawler.Model.Enemies;
using LabirynthCrawler.Model.Items;
using LabirynthCrawler.Model.Items.Weapons;
using LabirynthCrawler.Model.MapGeneration;
using LabirynthCrawler.Model.Observers;
using LabirynthCrawler.Model.PlayerModel;

namespace LabirynthCrawler.Model.Themes.ThemeCollection;

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
    public IMapGenerationStrategy GetStrategy() => new NetherGenerationStrategy();
    
    private readonly WeightedSpawnTable<IItem> _itemTable = new();
    private readonly WeightedSpawnTable<IWeapon> _weaponTable = new();
    private readonly WeightedSpawnTable<IEnemy> _enemyTable = new();
    private static readonly Random _rng = new Random();

    public NetherFactory(ISoundPublisher soundPublisher)
    {
        // Items
        _itemTable.AddEntry(() => new GoldNugget(), 30);
        _itemTable.AddEntry(() => new Coin(new Random().Next(1, 11)), 30);
        _itemTable.AddEntry(() => new Gold(new Random().Next(1, 4)), 20);
        _itemTable.AddEntry(() => new NetherQuartz(), 20);
        // Weapons
        _weaponTable.AddEntry(() => new GoldenSword(), weight: 50);
        _weaponTable.AddEntry(() => new NetheriteAxe(), weight: 30);
        _weaponTable.AddEntry(() => new BlazeRod(), weight: 20);
        
        // Enemies
        _enemyTable.AddEntry(() => new Piglin(new SpeciesFaction(), soundPublisher), weight: 60);
        _enemyTable.AddEntry(() => new WitherSkeleton(new SpeciesFaction(), soundPublisher), weight: 30);
        _enemyTable.AddEntry(() => new Blaze(new SpeciesFaction(), soundPublisher), weight: 10);
    }
    
    public string GetWelcomeMessage() => "Piekielne gorąco bije od ścian... Słyszysz grzechot kości Withera...";
    
    public IItem CreateItem() => _itemTable.Spawn();
    public IWeapon CreateWeapon() => _weaponTable.Spawn();
    public IItem CreateArtifact() => new WitherScythe();
    
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