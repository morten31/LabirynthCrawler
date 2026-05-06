using LabirynthCrawler.Model.Items.Weapons;
using LabirynthCrawler.Model.Items;

namespace LabirynthCrawler.Model.Combat.Visitors;

public interface IAttackVisitor
{
    (int Damage, int Defense) Visit(HeavyWeapon weapon);
    (int Damage, int Defense) Visit(LightWeapon weapon);
    (int Damage, int Defense) Visit(MagicWeapon weapon);
    (int Damage, int Defense) Visit(IItem? item);
}