using LabirynthCrawler.Model.Combat.Visitors;

namespace LabirynthCrawler.Model.Items.Weapons;

public interface IWeapon : IItem
{
    int Damage { get; }
}

public abstract class Weapon : ItemBase, IWeapon
{
    public override char GetSymbol() => 'W';
    public virtual int Damage { get; protected set; }
    
    public override string Name => $"{_name} ({Damage})"; 
    
    public Weapon(string name, int damage)
    {
        _name = name;
        Damage = damage;
    }

    public override string ToString() => Name;
    public override int GetSoundRange() => 3;
}

public abstract class HeavyWeapon : Weapon
{ 
    public HeavyWeapon(string name, int damage) : base(name, damage) { }

    public override bool IsTwoHanded () => true;

    public override int GetSoundRange() => 15;
    
    public override (int Damage, int Defense) Accept(IAttackVisitor visitor) => visitor.Visit(this);
}

public abstract class LightWeapon : Weapon
{ 
    public LightWeapon(string name, int damage) : base(name, damage) { }
    
    public override int GetSoundRange() => 10;

    public override (int Damage, int Defense) Accept(IAttackVisitor visitor) => visitor.Visit(this);

}

public abstract class MagicWeapon : Weapon
{ 
    public MagicWeapon(string name, int damage) : base(name, damage) { }
    
    public override int GetSoundRange() => 5;

    public override (int Damage, int Defense) Accept(IAttackVisitor visitor) => visitor.Visit(this);

}