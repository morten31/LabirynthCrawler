using LabirynthCrawler.Model.PlayerModel;

namespace LabirynthCrawler.Model.Items;

public class Gold : IPickable
{
    private int _ammount;
    public char GetSymbol() => 'G';

    public Gold(int ammount)
    {
        _ammount = ammount;
    }
    
    public override string ToString() => $"{_ammount}x Gold";


    public void OnPickUp(Player player)
    {
        player.GetInventory().ChangeGoldCount(_ammount);
    }
}

public class Coin : IPickable
{
    private int _ammount;
    public char GetSymbol() => 'C';

    public Coin(int ammount)
    {
        _ammount = ammount;
    }
    
    public override string ToString() => $"{_ammount}x Coin";

    public void OnPickUp(Player player)
    {
        player.GetInventory().ChangeCoinsCount(_ammount);
    }
}