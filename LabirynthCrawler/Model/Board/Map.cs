using LabirynthCrawler.Model.Items;

namespace LabirynthCrawler.Model.Board;

public class Map
{
    public const int Width = 40;
    public const int Heigth = 20;
    private readonly Tile[,] _map = new Tile[Heigth, Width];

    private void AddWall(int x, int y)
    {
        if(_map[y,x].ItemCount() == 0)
            _map[y, x].MakeWall();
    }
    public void AddItem(int x, int y, IPickable item)
    {
        if (!_map[y, x].IsWall())
        {
            _map[y, x].AddItem(item);
        }
    }

    
    public void Initialize()
    {
        for (int y = 0; y < Heigth; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                _map[y, x] = new Tile();
            }
        }
        
        AddItem(10, 15, new TwoHandedWeapons("Excalibur", 20));
        AddItem(5, 10, new Weapon("Dagger", 5));
        AddItem(10, 10, new TwoHandedWeapons("Longsword", 10));

        AddItem(6, 11, new Weapon("Dagger", 5));

        
        AddItem(20, 15, new QuestItem("Dragon's Egg"));
        AddItem(21, 16, new QuestItem("Dungeon Key"));

        AddItem(10, 9, new Coin(10));
        AddItem(10, 9, new Gold(2));

        AddWall(8, 7);
        AddWall(9, 7);
        AddWall(10, 7);
        AddWall(11, 7);
    }
    
    public Tile GetTile(int x, int y) => _map[y, x];

    public bool IsWithinBounds(int x, int y)
    {
        if (x < 0 || y < 0 || x >= Width || y >= Heigth)
            return false;

        return true;
    }
}