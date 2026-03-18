using LabirynthCrawler.Model.PlayerModel;

namespace LabirynthCrawler.Model.Items;

public interface IPickable
{
    char GetSymbol();

    void OnPickUp(Player player);
}