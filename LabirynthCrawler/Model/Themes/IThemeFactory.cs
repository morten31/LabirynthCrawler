using LabirynthCrawler.Model.Enemies;
using LabirynthCrawler.Model.Items;
using LabirynthCrawler.Model.Items.Weapons;
using LabirynthCrawler.Model.Observers;

namespace LabirynthCrawler.Model.Themes;

public interface IThemeFactory
{
    string GetWelcomeMessage();
    List<IEnemy> CreateEnemyGroup(ISoundPublisher soundPublisher); 
    IItem CreateItem();
    IItem CreateArtifact();
    IWeapon CreateWeapon();
    IMapGenerationStrategy GetStrategy();
}