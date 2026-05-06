
using LabirynthCrawler.Model.Combat.Visitors;
using LabirynthCrawler.Model.PlayerModel;

namespace LabirynthCrawler.Model.Items;

public abstract class ItemBase : IItem
{
    protected string _name;
    public virtual string Name => _name;
    
    public virtual bool IsTwoHanded() => false;
    public abstract char GetSymbol();

    public virtual void OnPickUp(Player player)
    {
        player.AddItem(this);
    }
    
    public virtual Attributes GetAttributes() => new(); 
    
    public override string ToString() => Name; 
    
    public virtual (int Damage, int Defense) Accept(IAttackVisitor visitor) => visitor.Visit(this);

    public virtual int GetSoundRange() => 0;
}
