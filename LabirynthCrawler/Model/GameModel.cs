using LabirynthCrawler.Model.Board;
using LabirynthCrawler.Model.Items;
using LabirynthCrawler.Model.MapGeneration;
using LabirynthCrawler.Model.PlayerModel;
using LabirynthCrawler.Model.MapGeneration;
namespace LabirynthCrawler.Model;


public enum Direction{ Up, Down, Left, Right }
public class GameModel
{
    private Map _map { get; set; } = new Map();
    private Player _player;
    public Player GetPlayer() => _player;
    public Map GetMap() => _map;
    InstructionBuilder _instructionBuilder;
    public InstructionBuilder GetInstructionBuilder() => _instructionBuilder;

    public void InitializeGame(int startX, int startY)
    {
        _player = new Player((startX, startY));
        var builder = new MapBuilder(startX, startY);
        _instructionBuilder = new InstructionBuilder();
        var director = new MapDirector();
        
        director.BuildStandardMap(builder);
        director.BuildStandardMap(_instructionBuilder);
        
        _map = builder.GetMap();
    }

    public void MovePlayer(Direction dir)
    {
        int oldX = _player.GetX();
        int oldY = _player.GetY();

        (int newX, int newY) = dir switch
        {
            Direction.Up => (oldX, oldY - 1),
            Direction.Down => (oldX, oldY + 1),
            Direction.Left => (oldX - 1, oldY),
            Direction.Right => (oldX + 1, oldY),
        };

        if (_map.IsWithinBounds(newX, newY) && !_map.GetTile(newX, newY).IsWall())
        {
            _player.MoveTo(newX, newY);
        }
    }

    public bool PickUpItem()
    {
        Tile tile = _map.GetTile(_player.GetX(), _player.GetY());
        if (tile.GetTileItems.Count <= 0)
            return false;
        IPickable item = tile.GetTileItems.First();
        item.OnPickUp(_player);
        tile.RemoveItem(tile.GetTileItems.IndexOf(item));
        return true;
    }

    public bool DropItem(int inIdx)
    {
        int idx = inIdx - 1;
        Inventory inventory = _player.GetInventory();
        if (idx < 0 || inventory.GetItemCount() < idx + 1)
            return false;
        
        IPickable item = inventory.RemoveFromInventory(idx);
        _map.AddItem(_player.GetX(), _player.GetY(), item);
        return true;
    }

    public bool EquipItem(char hand, int inIdx)
    {
        int idx = inIdx - 1;
        Inventory inventory = _player.GetInventory();
        if (idx < 0 || inventory.GetItemCount() < idx + 1)
            return false;
                
        inventory.EquipItem(hand, idx);
        return true;
    }
}
