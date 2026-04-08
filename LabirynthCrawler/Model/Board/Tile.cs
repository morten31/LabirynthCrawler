using LabirynthCrawler.Model.Enemies;
using LabirynthCrawler.Model.Items;

namespace LabirynthCrawler.Model.Board;

public class Tile
{
    private bool _isWall;
    private List<IItem> _items = new();
    private List<IEnemy> _enemies = new();

    public Tile(string type)
    {
        switch (type)
        {
            case "wall":
                _isWall = true;
                break;
            default:
                _isWall = false;
                break;
        }
    }

    public string GetSymbol()
    {
        if (_isWall)
            return "█";
        else if (_enemies.Count > 0) return _enemies[0].GetSymbol();
        else if (_items.Count == 0)
            return " ";
        else
            return $"{_items[0].GetSymbol()}";
    }

    public void AddItem(IItem item) => _items.Add(item);
    public int ItemCount() => _items.Count;

    public List<IItem> GetTileItems => _items;
    
    public IItem RemoveItem(int idx)
    {
        IItem item = _items[idx];
        _items.RemoveAt(idx);
        return item;
    }
    
    public void AddEnemy(IEnemy enemy) => _enemies.Add(enemy);
    public List<IEnemy> GetEnemies() => _enemies;
    public void RemoveDeadEnemies() => _enemies.RemoveAll(e => e.GetHealth() <= 0);
    
    
    public bool IsWall() => _isWall;
    public void MakeWall() => _isWall = true;
}