using LabirynthCrawler.Model.Board;
using LabirynthCrawler.Model.Items;

namespace LabirynthCrawler.Model.MapGeneration;

public class MapBuilder : IMapBuilder
{
    protected Map _map;
    protected readonly (int, int) _playerStart;
    protected bool _firstStepDone;
    protected readonly List<(int, int)> _FloorList = new();


    public MapBuilder(Map map, int startX, int startY)
    {
        _map = map;
        _playerStart = (startX, startY);
        _firstStepDone = false;
    }

    public MapBuilder(int startX, int startY)
    {
        _map = new Map();
        _playerStart = (startX, startY);
        _firstStepDone = false;
    }

    public Map GetMap() => _map;
    
    protected static readonly Random _rng = new Random();
    protected readonly (int dx, int dy)[] _directions =
    {
        (0, 1), (-1, 0), (0, -1), (1, 0)
    };
    
    
    public IMapBuilder BuildEmpty()
    {
        for (int y = 0; y < Map.Height; y++)
        {
            for (int x = 0; x < Map.Width; x++)
            {
                SetToFloor(x, y);
            }
        }
        _firstStepDone = true;
        
        return this;
    }

    public IMapBuilder FillWithWalls()
    {
        for (int y = 0; y < Map.Height; y++)
        {
            for (int x = 0; x < Map.Width; x++)
            {
                SetToWall(x, y);
            }
        }    
        
        _firstStepDone = true;
        
        return this;
    }

    public virtual IMapBuilder AddCorridors()
    { 
        if (!_firstStepDone) return this; 
        
        var (px, py) = _playerStart;
        if (!_map.IsWithinBounds(px, py)) return this;

        var stack = new Stack<(int, int)>();
        var visited = new bool[Map.Height, Map.Width];
        visited[py, px] = true;
        SetToFloor(px,py);
        stack.Push((px, py));

        while (stack.Count > 0)
        { 
            var (x, y) = stack.Peek();
            var neighbours = new List<(int dx, int dy)>();
            foreach (var (dx, dy) in _directions.OrderBy(_ => _rng.Next()))
            {
                int newx = x + (2 * dx);
                int newy = y + (2 * dy);
                if (_map.IsWithinBounds(newx, newy) && !visited[newy, newx])
                { 
                    neighbours.Add((dx, dy));
                }
            }

            if (neighbours.Count > 0)
            {
                var (dx, dy) = neighbours.First();
                SetToFloor(x + dx, y + dy);
                SetToFloor(x + (2 * dx), y + (2 * dy));
                visited[y + (2 * dy), x + (2 * dx)] = true;

                stack.Push((x + (2 * dx), y + (2 * dy)));
            }
            else stack.Pop();
        }
        MakeHoles(5);
        
        return this;
    } 
    
    protected void MakeHoles(int chancePercentage)
    { 
        for (int y = 1; y < Map.Height - 1; y++)
        { 
            for (int x = 1; x < Map.Width - 1; x++)
            {
                if (_map.GetTile(x, y).IsWall())
                {
                    bool horizontalSpace = !_map.GetTile(x - 1, y).IsWall() && !_map.GetTile(x + 1, y).IsWall();
                    bool verticalSpace = !_map.GetTile(x, y - 1).IsWall() && !_map.GetTile(x, y + 1).IsWall();

                    if ((horizontalSpace || verticalSpace) && _rng.Next(100) < chancePercentage)
                    {
                        SetToFloor(x, y);
                    }
                }
            }
        }
    }
    
    
    public IMapBuilder AddMainRoom(int sizeX, int sizeY)
    {
        if (!_firstStepDone) return this; 

        int rx = (Map.Width - sizeX)/2;
        int ry = (Map.Height - sizeY)/2;
        MakeRoom(rx, ry, sizeX, sizeY);
        return this;
    }

    public IMapBuilder AddRooms(int count, int size)
    {
        if (!_firstStepDone) return this; 
        
        for (int i = 0; i < count; i++)
        {
            int rx = _rng.Next(1, Map.Width - size - 1);
            int ry = _rng.Next(1, Map.Height - size - 1);
            MakeRoom(rx, ry, size, size);
        }
        return this;
    }

    public IMapBuilder AddItems(int count)
    {
        if (!_firstStepDone) return this; 

        for (int i = 0; i < count && TryGetFloor(out var pos); i++)
        {
            IPickable item = _rng.Next(_items.Length) switch
            {
                0 => new Coin(_rng.Next(1, 10)),
                1 => new Gold(_rng.Next(1, 3)),
                2 => new DungeonKey(),
                3 => new DragonEgg(),
                _ => new TrollSkull()
            };
            _map.AddItem(pos.x, pos.y, item);
        }
        return this;
    }

    public IMapBuilder AddWeapons(int count)
    {
        if (!_firstStepDone) return this; 

        for (int i = 0; i < count && TryGetFloor(out var pos); i++)
        {
            Weapon weapon = _rng.Next(_weapons.Length) switch
            {
                0 => new Dagger(),
                1 => new Excalibur(),
                _ => new Longsword()
            };
            _map.AddItem(pos.x, pos.y, weapon);
        }
        
        return this;
    }

    public void MakeRoom(int x1, int y1, int width, int height)
    {
        for (int x = x1; x < x1 + width; x++)
        {
            for (int y = y1; y < y1 + height; y++)
            {
                SetToFloor(x, y);
            }
        }   
    }

    private bool TryGetFloor(out (int x, int y) pos)
    {
        if (_FloorList.Count == 0)
        {
            pos = (-1, -1);
            return false;
        }

        int idx = _rng.Next(_FloorList.Count);
        var t = _FloorList[idx];
        _FloorList.RemoveAt(idx);
        pos = t;
        return true;
    }
    
    
    public void SetToFloor(int x, int y)
    {
        _map.SetTileEmpty(x, y);
        if(!_FloorList.Contains((x, y)))
            _FloorList.Add((x, y)); 
    }
    
    public void SetToWall(int x, int y)
    {
        _map.SetTileWall(x, y);
        if (_FloorList.Contains((x, y)))
            _FloorList.Remove((x, y));
    }
    
    private readonly string[] _items = { "Coin", "Gold", "DungeonKey", "TrollSkull", "DragonEgg" };
    private readonly string[] _weapons = { "Dagger", "Excalibur", "Longsword" };

}