using LabirynthCrawler.Model.Board;
using LabirynthCrawler.Model.Combat;
using LabirynthCrawler.Model.Enemies;
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

    public string LastActionLog { get; set; } = "";
    public bool IsGameOver { get; private set; } = false;
    
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
        IItem item = tile.GetTileItems.First();
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
        
        IItem item = inventory.RemoveFromInventory(idx);
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

    public void PerformAttack(int attackType)
    {
        if (IsGameOver) return;

        Tile currentTile = GetMap().GetTile(_player.GetX(), _player.GetY());
        List<IEnemy> enemies = currentTile.GetEnemies();

        if (enemies.Count == 0)
        {
            LastActionLog = "There are no enemies to attack!";
            return;
        }

        IEnemy enemy = enemies[0];
        CombatSystem combat = new CombatSystem(_player, enemy);
        CombatResult result = combat.AttackEnemy(attackType);

        LastActionLog = $"[COMBAT] You dealt {result.DamageDealt} dmg to {enemy.ToString()}. ";

        if (result.IsEnemyDead)
        {
            LastActionLog += $"{enemy.ToString()} was killed! ";
            currentTile.RemoveDeadEnemies();
        }
        else
        {
            LastActionLog += $"{enemy.ToString()} hit back for {result.DamageReceived} dmg. ";
        }

        if (result.IsPlayerDead)
        {
            LastActionLog += "YOU DIED! Game Over. Press ESC to quit.";
            IsGameOver = true;
        }
    }
}
