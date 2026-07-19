using LabirynthCrawler.Model.Enemies;
using LabirynthCrawler.Model.Items;
using LabirynthCrawler.Model.Items.Weapons;
using LabirynthCrawler.Model.MapGeneration;
using LabirynthCrawler.Model.Observers;
using LabirynthCrawler.Model.PlayerModel;

namespace LabirynthCrawler.Model.Themes.ThemeCollection;

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
    public IMapGenerationStrategy GetStrategy(){ return new EndGenerationStrategy(); }
    
    private readonly WeightedSpawnTable<IItem> _itemTable = new();
    private readonly WeightedSpawnTable<IWeapon> _weaponTable = new();
    private readonly WeightedSpawnTable<IEnemy> _enemyTable = new();
    private static readonly Random _rng = new Random();

    public EndFactory(ISoundPublisher soundPublisher)
    {
        // Items 
        _itemTable.AddEntry(() => new EnderPearl(), 70);
        _itemTable.AddEntry(() => new ChorusFruit(), 30);
        // Weapons
        _weaponTable.AddEntry(() => new VoidSword(), weight: 50);
        _weaponTable.AddEntry(() => new ObsidianMace(), weight: 30);
        _weaponTable.AddEntry(() => new ShulkerShield(), weight: 20);
        // Enemies
        _enemyTable.AddEntry(() => new Enderman(new SpeciesFaction(), soundPublisher), weight: 60);
        _enemyTable.AddEntry(() => new Shulker(new SpeciesFaction(), soundPublisher), weight: 30);
        _enemyTable.AddEntry(() => new EnderDragon(new SpeciesFaction(), soundPublisher), weight: 10);
    }
    
    public string GetWelcomeMessage() => "Otacza Cię bezkresna pustka. Patrz pod nogi i... nie patrz Czarnym w oczy.";

    public IItem CreateItem() => _itemTable.Spawn();
    public IWeapon CreateWeapon() => _weaponTable.Spawn();
    public IItem CreateArtifact() => new DragonSlayerHalberd();
    
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
