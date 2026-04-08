using LabirynthCrawler.Model.PlayerModel;
using LabirynthCrawler.Model.Items.Weapons;
using LabirynthCrawler.Model.Combat.Visitors;

namespace LabirynthCrawler.Model.Items.Decorators;

public class StrongModifier : WeaponDecorator
{
    public StrongModifier(IWeapon wrappee) : base(wrappee) { }

    public override string Name => $"{_weapon.Name} (Strong)";

    public override int Damage => _weapon.Damage + 5;
    
    public override (int Damage, int Defense) Accept(IAttackVisitor visitor)
    {
        var (baseDmg, def) = base.Accept(visitor);
        return (baseDmg > 0 ? baseDmg + 5 : 0, def); 
    }
}

public class UnluckyModifier : WeaponDecorator
{
    public UnluckyModifier(IWeapon wrappee) : base(wrappee) { }

    public override string Name => $"{_weapon.Name} (Unlucky)";

    public override Attributes GetAttributes()
    {
        Attributes baseAttr = _weapon.GetAttributes();
        Attributes modifierAttr = new Attributes(luck: -5);
        
        return baseAttr.Add(modifierAttr);
    }
}

public class AgileModifier : WeaponDecorator
{
    public AgileModifier(IWeapon wrappee) : base(wrappee) { }

    public override string Name => $"{_weapon.Name} (Agile)";

    public override Attributes GetAttributes()
    {
        Attributes baseAttr = _weapon.GetAttributes();
        Attributes modifierAttr = new Attributes(agility: 5);
        
        return baseAttr.Add(modifierAttr);
    }
}