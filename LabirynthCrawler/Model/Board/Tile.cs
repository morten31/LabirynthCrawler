using LabirynthCrawler.Model.Items;

namespace LabirynthCrawler.Model.Board;

public class Tile
{
    private bool _isWall;
    private List<IPickable> _items = new();

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
        else if (_items.Count == 0)
            return " ";
        else
            return $"{_items[0].GetSymbol()}";
    }

    public void AddItem(IPickable item) => _items.Add(item);
    public int ItemCount() => _items.Count;

    public List<IPickable> GetTileItems => _items;
    
    public IPickable RemoveItem(int idx)
    {
        IPickable item = _items[idx];
        _items.RemoveAt(idx);
        return item;
    }

    public bool IsWall() => _isWall;
    public void MakeWall() => _isWall = true;
}