namespace LabirynthCrawler.Model.Items;

public abstract class Weapon : ItemBase
{
    public override char GetSymbol() => 'W';
    public int Damage { get; protected set; }
    
    public Weapon(string name, int damage)
    {
        _name = name;
        Damage = damage;
    }

    public override string ToString() => $"{_name} ({Damage})";

}

public abstract class TwoHandedWeapon : Weapon
{ 
    public TwoHandedWeapon(string name, int damage) : base(name, damage) { }

    public override bool IsTwoHanded () => true;
}

public class Longsword : TwoHandedWeapon
{
    public Longsword() : base("Longsword", 20) { }
}

public class Excalibur : TwoHandedWeapon
{
    public Excalibur() : base("Excalibur", 50) { }
}

public class Dagger : Weapon
{
    public Dagger() : base("Dagger", 10) { }
}