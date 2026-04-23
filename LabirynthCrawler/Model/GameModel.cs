using LabirynthCrawler.Model.Board;
using LabirynthCrawler.Model.Combat;
using LabirynthCrawler.Model.Enemies;
using LabirynthCrawler.Model.Items;
using LabirynthCrawler.Model.Logger;
using LabirynthCrawler.Model.MapGeneration;
using LabirynthCrawler.Model.PlayerModel;
using LabirynthCrawler.Model.MapGeneration;
using LabirynthCrawler.Model.Themes;

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

    public enum GameState
    {
        Playing,
        ViewingLog,
        GameOver
    }
    
    public GameState CurrentState { get; set; } = GameState.Playing;
    
    public bool IsGameOver { get; private set; } = false;
    
    public void InitializeGame(int startX, int startY, string themeName)
    {
        _player = new Player((startX, startY));
        _instructionBuilder = new InstructionBuilder();
        
        IThemeFactory factory;
        IMapGenerationStrategy strategy;

        switch (themeName.ToLower())
        {
            case "overworld":
                factory = new OverworldFactory();
                strategy = new OverworldGenerationStrategy();
                break;
            case "nether":
                factory = new NetherFactory();
                strategy = new NetherGenerationStrategy();
                break;
            case "end":
                factory = new EndFactory();
                strategy = new EndGenerationStrategy();
                break;
            default:
                factory = new OverworldFactory(); 
                strategy = new OverworldGenerationStrategy();
                break;
        }
        
        var builder = new MapBuilder(startX, startY, factory);

        strategy.Generate(builder);
        strategy.Generate(_instructionBuilder);
        
        _map = builder.GetMap();
        
        GameLogger.Instance.Log($"--- {factory.GetWelcomeMessage()} ---");
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

        if (_map.IsWithinBounds(newX, newY))
        {
            if (_map.GetTile(newX, newY).IsWall())
            {
                GameLogger.Instance.Log("Failed! A wall is on the way!");
            }
            else
            {
                _player.MoveTo(newX, newY);
            }
        }
    }

    public bool PickUpItem()
    {
        Tile tile = _map.GetTile(_player.GetX(), _player.GetY());
        if (tile.GetTileItems.Count <= 0)
        {
            GameLogger.Instance.Log("No item to pick up!");
            return false;
        }
        IItem item = tile.GetTileItems.First();
        item.OnPickUp(_player);
        tile.RemoveItem(tile.GetTileItems.IndexOf(item));
        GameLogger.Instance.Log($"Picked up item: {item.Name}");
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
        GameLogger.Instance.Log($"Dropped item: {item.Name}");
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
            GameLogger.Instance.Log("There are no enemies to attack!");
            return;
        }

        IEnemy enemy = enemies[0];
        CombatSystem combat = new CombatSystem(_player, enemy);
        CombatResult result = combat.AttackEnemy(attackType);

        GameLogger.Instance.Log($"[COMBAT] You dealt {result.DamageDealt} dmg to {enemy.ToString()}. ");

        if (result.IsEnemyDead)
        {
            GameLogger.Instance.Log($"{enemy.ToString()} was killed!");
            currentTile.RemoveDeadEnemies();
        }
        else
        {
            GameLogger.Instance.Log($"{enemy.ToString()} hit back for {result.DamageReceived} dmg.");
        }

        if (result.IsPlayerDead)
        {
            GameLogger.Instance.Log("YOU DIED! Game Over. Press ESC to quit.");
            CurrentState = GameState.GameOver;
        }
    }
}
