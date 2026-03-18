using LabirynthCrawler.Model.Board;
using LabirynthCrawler.Model.Items;
using LabirynthCrawler.Model.PlayerModel;
namespace LabirynthCrawler.Model;


public enum Direction{ Up, Down, Left, Right }
public class GameModel
{
    private Map map { get; set; } = new Map();
    private Player player { get; set; } = new((0,0));
    public Player GetPlayer() => player;
    public Map GetMap() => map;

    public void InitializeGame()
    {
        map.Initialize();
        
    }

    public void MovePlayer(Direction dir)
    {
        int oldX = player.GetX();
        int oldY = player.GetY();

        (int newX, int newY) = dir switch
        {
            Direction.Up => (oldX, oldY - 1),
            Direction.Down => (oldX, oldY + 1),
            Direction.Left => (oldX - 1, oldY),
            Direction.Right => (oldX + 1, oldY),
        };

        if (map.IsWithinBounds(newX, newY) && !map.GetTile(newX, newY).IsWall())
        {
            player.MoveTo(newX, newY);
        }
    }

    public bool PickUpItem()
    {
        Tile tile = map.GetTile(player.GetX(), player.GetY());
        if (tile.GetTileItems.Count <= 0)
            return false;
        IPickable item = tile.GetTileItems.First();
        item.OnPickUp(player);
        tile.RemoveItem(tile.GetTileItems.IndexOf(item));
        return true;
    }

    public bool DropItem(int inIdx)
    {
        int idx = inIdx - 1;
        Inventory inventory = player.GetInventory();
        if (idx < 0 || inventory.GetItemCount() < idx + 1)
            return false;
        
        IPickable item = inventory.RemoveFromInventory(idx);
        map.AddItem(player.GetX(), player.GetY(), item);
        return true;
    }

    public bool EquipItem(char hand, int inIdx)
    {
        int idx = inIdx - 1;
        Inventory inventory = player.GetInventory();
        if (idx < 0 || inventory.GetItemCount() < idx + 1)
            return false;
                
        inventory.EquipItem(hand, idx);
        return true;
    }
}
