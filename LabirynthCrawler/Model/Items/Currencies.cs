using LabirynthCrawler.Model.PlayerModel;

namespace LabirynthCrawler.Model.Items;

public class Gold : ItemBase
{
    private int _ammount;
    public override char GetSymbol() => 'G';

    public Gold(int ammount)
    {
        _ammount = ammount;
    }
    
    public override string Name => $"{_ammount}x Gold";
    
    public override void OnPickUp(Player player)
    {
        player.GetInventory().ChangeGoldCount(_ammount);
    }
    
    public override string ToString() => Name; 
}

public class Coin : ItemBase
{
    private int _ammount;
    public override char GetSymbol() => 'C';

    public Coin(int ammount)
    {
        _ammount = ammount;
    }
    
    public override string Name => $"{_ammount}x Coin";

    public override void OnPickUp(Player player)
    {
        player.GetInventory().ChangeCoinsCount(_ammount);
    }
    public override string ToString() => Name; 
}