using LabirynthCrawler.Model.Combat.Visitors;
using LabirynthCrawler.Model.PlayerModel;
using LabirynthCrawler.Model.Items.Weapons;

namespace LabirynthCrawler.Model.Items.Decorators;

public abstract class WeaponDecorator : IWeapon
{
    protected IWeapon _weapon;

    public WeaponDecorator(IWeapon wrappee)
    {
        _weapon = wrappee;
    }
    public virtual string Name => _weapon.Name;
    public virtual char GetSymbol() => _weapon.GetSymbol();
    public virtual bool IsTwoHanded() => _weapon.IsTwoHanded();
    public virtual Attributes GetAttributes() => _weapon.GetAttributes();
    public virtual int Damage => _weapon.Damage;

    public virtual void OnPickUp(Player player) => player.AddItem(this); 
    
    public override string ToString() => Name;
    
    public virtual (int Damage, int Defense) Accept(IAttackVisitor visitor) 
        => _weapon.Accept(visitor);
}
