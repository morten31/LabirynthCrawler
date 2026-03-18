
using LabirynthCrawler.Model.PlayerModel;

namespace LabirynthCrawler.Model.Items;

public abstract class ItemBase : IPickable
{
    protected string _name;
    public virtual bool IsTwoHanded() => false;
    public override string ToString() => _name;
    public abstract char GetSymbol();

    public void OnPickUp(Player player)
    {
        player.AddItem(this);
    }
}
