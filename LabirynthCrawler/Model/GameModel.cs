using LabirynthCrawler.Model.Board;
using LabirynthCrawler.Model.Logger;
using LabirynthCrawler.Model.MapGeneration;
using LabirynthCrawler.Model.PlayerModel;
using LabirynthCrawler.Model.Observers;
using LabirynthCrawler.Model.Themes;
using LabirynthCrawler.Model.Themes.ThemeCollection;

namespace LabirynthCrawler.Model;

public enum Direction{ Up, Down, Left, Right }

public class GameModel
{
    public readonly object StateLock = new object();

    private Map _map { get; set; } = new Map();
    
    private Dictionary<int, Player> _players = new Dictionary<int, Player>();
    public IReadOnlyDictionary<int, Player> Players => _players;
    
    public Player? GetPlayer(int playerId) => _players.ContainsKey(playerId) ? _players[playerId] : null;
    public Map GetMap() => _map;

    public enum GameState
    {
        Playing,
        GameOver
    }
    
    public GameState CurrentState { get; set; } = GameState.Playing;
    
    public bool IsGameOver { get; private set; } = false;
    
    public void InitializeGame(int startX, int startY, string themeName)
    {
        var soundManager = new SoundManager();
        _players[1] = new Player((startX, startY), soundManager);
        
        IThemeFactory factory = themeName.ToLower() switch
        {
            "nether" => new NetherFactory(soundManager),
            "end" => new EndFactory(soundManager),
            _ => new OverworldFactory(soundManager)
        };
        
        var builder = new MapBuilder(startX, startY, factory, soundManager);

        IMapGenerationStrategy strategy = factory.GetStrategy();
        strategy.Generate(builder);
        
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
}