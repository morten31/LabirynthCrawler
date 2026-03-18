namespace LabirynthCrawler.Model.Items;

public class Weapon : ItemBase
{
    public override char GetSymbol() => 'V';
    public int Damage { get; private set; }

    public Weapon(string name, int damage)
    {
        _name = name;
        Damage = damage;
    }
    public override string ToString() => $"{_name} ({Damage})";

}

public class TwoHandedWeapons : Weapon
{
    public TwoHandedWeapons(string name, int damage) : base(name, damage) { }
    public override bool IsTwoHanded () => true;
    public override char GetSymbol() => 'W';

}

//
// public class Longsword : ItemBase, IWeapon
// {
//     private const int Damage = 20;
//     public int GetDamage() => Damage;
//     public override string GetName() => "Longsword";   
//     public override char GetSymbol() => "W";
//
// }
//
// public class Excalibur : ItemBase, IWeapon
// {
//     private const int Damage = 50;
//     public int GetDamage() => Damage;
//     public override string GetName() => "Excalibur";   
//     public override char GetSymbol() => "W";
// }
//
// public class Dagger : ItemBase, IWeapon
// {
//     private const int Damage = 10;
//     public int GetDamage() => Damage;
//     public override string GetName() => "Dagger";    
//     public override char GetSymbol() => "W";
// }