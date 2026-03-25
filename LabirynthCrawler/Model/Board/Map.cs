using LabirynthCrawler.Model.Items;
using LabirynthCrawler.Model.MapGeneration;

namespace LabirynthCrawler.Model.Board;

public class Map
{
    public const int Width = 41;
    public const int Height = 21;
    private readonly Tile[,] _map = new Tile[Height, Width];

    public void SetTileEmpty(int x, int y) => _map[y, x] = new Tile("empty");
    
    
    public void SetTileWall(int x, int y) => _map[y, x] = new Tile("wall");
    
    public void AddItem(int x, int y, IPickable item)
    {
        if (!_map[y, x].IsWall())
        {
            _map[y, x].AddItem(item);
        }
    }
    
    public Tile GetTile(int x, int y) => _map[y, x];

    public bool IsWithinBounds(int x, int y)
    {
        if (x < 0 || y < 0 || x >= Width || y >= Height)
            return false;

        return true;
    }
}