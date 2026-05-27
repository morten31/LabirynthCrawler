using LabirynthCrawler.Model.Board;
using LabirynthCrawler.Model.Combat;
using LabirynthCrawler.Model.Enemies;
using LabirynthCrawler.Model.Items;
using LabirynthCrawler.Model.Logger;
using LabirynthCrawler.Model.MapGeneration;
using LabirynthCrawler.Model.PlayerModel;
using LabirynthCrawler.Model.MapGeneration;
using LabirynthCrawler.Model.Observers;
using LabirynthCrawler.Model.Themes;

namespace LabirynthCrawler.Model;


public enum Direction{ Up, Down, Left, Right }
public class GameModel
{
    public readonly object StateLock = new object();

    private Map _map { get; set; } = new Map();
    
    private Dictionary<int, Player> _players = new Dictionary<int, Player>();
    public IReadOnlyDictionary<int, Player> Players => _players;
    
    private Player _player;
    public Player? GetPlayer(int playerId) => _players.ContainsKey(playerId) ? _players[playerId] : null;
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
        var soundManager = new SoundManager();
        _players[1] = new Player((startX, startY), soundManager);
        _instructionBuilder = new InstructionBuilder();
        
        IThemeFactory factory = themeName.ToLower() switch
        {
            "nether" => new NetherFactory(),
            "end" => new EndFactory(),
            _ => new OverworldFactory()
        };
        
        var builder = new MapBuilder(startX, startY, factory, soundManager);

        IMapGenerationStrategy strategy = factory.GetStrategy();
        strategy.Generate(builder);
        strategy.Generate(_instructionBuilder);
        
        _map = builder.GetMap();
        soundManager.Initialize(_map);
        
        GameLogger.Instance.Log($"-- {factory.GetWelcomeMessage()} --");
    }
    
    public void AddPlayer(int playerId, int x, int y)
    {
        var soundManager = new SoundManager();
        soundManager.Initialize(_map);
        _players[playerId] = new Player((x, y), soundManager);
    }
    
    public void RemovePlayer(int playerId)
    {
        lock (StateLock)
        {
            if (_players.ContainsKey(playerId))
            {
                _players.Remove(playerId);
            }
        }
    }
    
    public void Tick()
    {
        lock (StateLock)
        {
            var allEnemies = _map.GetAllEnemies().ToList();
            foreach (var enemy in allEnemies)
            {
                enemy.MoveRandomly(_map);
            }
        }
    }

    public void MovePlayer(int playerId, Direction dir)
    {
        if (!_players.ContainsKey(playerId)) return;
        Player p = _players[playerId];

        
        int oldX = p.GetX();
        int oldY = p.GetY();

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
                GameLogger.Instance.Log("Failed! A wall is on the way!", LogLevel.Trace, playerId);
            }
            else
            {
                p.MoveTo(newX, newY);
            }
        }
    }

    public bool PickUpItem(int playerId)
    {
        lock (StateLock)
        {
            if (!_players.ContainsKey(playerId)) return false;
            Player p = _players[playerId];
            if (p.IsDead) return false;

            Tile tile = _map.GetTile(p.GetX(), p.GetY());
            if (tile.GetTileItems.Count <= 0)
            {
                GameLogger.Instance.Log("No item to pick up!", LogLevel.Trace, playerId);
                return false;
            }

            IItem item = tile.GetTileItems.First();
            item.OnPickUp(p);
            tile.RemoveItem(tile.GetTileItems.IndexOf(item));
            p.MakeNoise(item);
            GameLogger.Instance.Log($"Picked up item: {item.Name}", LogLevel.Info, playerId);
            return true;
        }
    }

    public bool DropItem(int playerId, int inIdx)
    {
        lock (StateLock)
        {
            if (!_players.ContainsKey(playerId)) return false;
            Player p = _players[playerId];
            if (p.IsDead) return false;
            
            int idx = inIdx - 1;
            Inventory inventory = p.GetInventory();
            if (idx < 0 || inventory.GetItemCount() < idx + 1)
                return false;
            
            IItem item = inventory.RemoveFromInventory(idx);
            _map.AddItem(p.GetX(), p.GetY(), item);
            GameLogger.Instance.Log($"Dropped item: {item.Name}");
            return true;
        }
    }

    public bool EquipItem(int playerId, char hand, int inIdx)
    {
        lock (StateLock)
        {
            if (!_players.ContainsKey(playerId)) return false;
            Player p = _players[playerId];
            if (p.IsDead) return false;


            int idx = inIdx - 1;
            Inventory inventory = p.GetInventory();
            if (idx < 0 || inventory.GetItemCount() < idx + 1)
                return false;

            inventory.EquipItem(hand, idx);
            
            GameLogger.Instance.Log($"Equipped item in hand", LogLevel.Info, playerId);
            return true;
        }
    }

    public void PerformAttack(int playerId, int attackType)
    {
        lock (StateLock)
        {
            if (!_players.ContainsKey(playerId)) return;
            Player p = _players[playerId];
            if (p.IsDead) return;
            
            Tile currentTile = GetMap().GetTile(p.GetX(), p.GetY());
            List<IEnemy> enemies = currentTile.GetEnemies();

            if (enemies.Count == 0)
            {
                GameLogger.Instance.Log("There are no enemies to attack!", LogLevel.Trace, playerId);
                return;
            }

            IEnemy enemy = enemies[0];
            CombatSystem combat = new CombatSystem(p, enemy);
            CombatResult result = combat.AttackEnemy(attackType);

            GameLogger.Instance.Log($"You dealt {result.DamageDealt} dmg to {enemy.ToString()}.", LogLevel.Combat, playerId);

            if (result.IsPlayerDead)
            {
                GameLogger.Instance.Log($"Player {playerId} was killed by {enemy.ToString()}!", LogLevel.System);
    
                GameLogger.Instance.Log("YOU DIED! Spectating mode.", LogLevel.System, playerId);
    
                p.IsDead = true;

                if (_players.Values.All(player => player.IsDead))
                {
                    GameLogger.Instance.Log("All players dead! Ending Game...", LogLevel.System);
                    CurrentState = GameState.GameOver;
                }
            }
            else
            {
                GameLogger.Instance.Log($"{enemy.ToString()} hit back for {result.DamageReceived} dmg.", LogLevel.Combat, playerId);
            }

            if (result.IsPlayerDead)
            {
                GameLogger.Instance.Log("YOU DIED! Spectating mode.", LogLevel.System, playerId);
                p.IsDead = true;

                if (_players.Values.All(player => player.IsDead))
                {
                    GameLogger.Instance.Log("All players dead! Ending Game...", LogLevel.System);
                    CurrentState = GameState.GameOver;
                }            }
        }
    }
}
