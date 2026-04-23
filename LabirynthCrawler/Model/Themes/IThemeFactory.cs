using LabirynthCrawler.Model.Enemies;
using LabirynthCrawler.Model.Items;
using LabirynthCrawler.Model.Items.Weapons;

namespace LabirynthCrawler.Model.Themes;

public interface IThemeFactory
{
    string GetWelcomeMessage();
    IEnemy CreateEnemy();
    IItem CreateItem();
    IItem CreateArtifact();
    IWeapon CreateWeapon();
}