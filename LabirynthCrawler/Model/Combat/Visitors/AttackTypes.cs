using LabirynthCrawler.Model.Items;
using LabirynthCrawler.Model.Items.Weapons;
using LabirynthCrawler.Model.PlayerModel;

namespace LabirynthCrawler.Model.Combat.Visitors;

public class NormalAttackVisitor(Player player) : IAttackVisitor
{
    Attributes _attr = player.GetTotalAttributes();

    public (int Damage, int Defense) Visit(HeavyWeapon weapon) => (weapon.Damage + _attr.Power + _attr.Aggression, _attr.Power + _attr.Luck);
    public (int Damage, int Defense) Visit(LightWeapon weapon) => (weapon.Damage + _attr.Agility + _attr.Luck, _attr.Agility + _attr.Luck);
    public (int Damage, int Defense) Visit(MagicWeapon weapon) => (1, _attr.Agility + _attr.Luck);
    public (int Damage, int Defense) Visit(IItem? item) => (0, _attr.Agility);
}

public class StealthAttackVisitor(Player player) : IAttackVisitor
{
    Attributes _attr = player.GetTotalAttributes();

    public (int Damage, int Defense) Visit(HeavyWeapon weapon) => ((weapon.Damage + _attr.Power + _attr.Aggression) / 2, _attr.Power);
    public (int Damage, int Defense) Visit(LightWeapon weapon) => ((weapon.Damage + _attr.Agility + _attr.Luck) * 2, _attr.Agility);
    public (int Damage, int Defense) Visit(MagicWeapon weapon) => (1, 0);
    public (int Damage, int Defense) Visit(IItem? item) => (0, 0);
}

public class MagicAttackVisitor(Player player) : IAttackVisitor
{
    Attributes _attr = player.GetTotalAttributes();

    public (int Damage, int Defense) Visit(HeavyWeapon weapon) => (1, _attr.Luck);
    public (int Damage, int Defense) Visit(LightWeapon weapon) => (1, _attr.Luck);
    public (int Damage, int Defense) Visit(MagicWeapon weapon) => (weapon.Damage + _attr.Wisdom, _attr.Wisdom * 2);
    public (int Damage, int Defense) Visit(IItem? item) => (0, _attr.Luck);
}