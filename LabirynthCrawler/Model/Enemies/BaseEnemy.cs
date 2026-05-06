using LabirynthCrawler.Model.Board;
using LabirynthCrawler.Model.Logger;
using LabirynthCrawler.Model.Observers;

namespace LabirynthCrawler.Model.Enemies;

public abstract class BaseEnemy : IEnemy, IDeathObserver, ISoundObserver
{
    protected int _x, _y;
    protected string _name;
    protected int _health;
    protected int _maxHealth;
    protected int _damage;
    protected int _defence;
    protected ISpeciesPublisher _faction;
    protected ISoundPublisher _soundPublisher;

    protected BaseEnemy(string name, int health, int damage, int defence, ISpeciesPublisher faction, ISoundPublisher soundPublisher)
    {
        _name = name;
        _health = health;
        _maxHealth = health;
        _damage = damage;
        _defence = defence;
        _faction = faction;
        _soundPublisher = soundPublisher;
        _faction.Attach(this);
        _soundPublisher.Attach(this);
    }
    
    public void HandleOwnDeath()
    {
        _faction.Detach(this);
        _faction.NotifyDeath();
        _soundPublisher.Detach(this);
    }
    
    public void OnAllyDeath()
    {
        if (_health > 0)
        {
            ReactToAllyDeath();
        }
    }

    public virtual void ReactToAllyDeath() 
    {
    }
    
    public void OnSoundHeard(int sourceX, int sourceY, int distance, string sourceName)
    {
        if (_health > 0)
        {
            GameLogger.Instance.Log($"[{_name} at ({_x},{_y})] heard ({sourceName}) from distance {distance}.");
        }
    }
    
    public int GetX() => _x;
    public int GetY() => _y;
    
    public virtual string GetSymbol() => "X";
    public override string ToString() => $"{_name} ({_health}/{_maxHealth})";
    
    public int GetDamage() => _damage;
    public int GetHealth() => _health;
    public int GetDefence() => _defence;
    
    public void DecreaseHealth(int damage)
    {
        int actualDamage = Math.Max(0, damage - _defence);
        _health -= damage;
        if (_health < 0) _health = 0;
    }

    public int GetMaxHealth() => _maxHealth;
    
    public void SetPosition(int x, int y) 
    { 
        _x = x; 
        _y = y; 
    }

    public void MoveRandomly(Map map)
    {
        if (_health <= 0) return;

        var random = new Random();
        if (random.Next(2) % 2 == 0) return;
        
        var directions = new (int dx, int dy)[] { (0, 1), (0, -1), (1, 0), (-1, 0) };
        var validMoves = new List<(int x, int y)>();

        foreach (var dir in directions)
        {
            int newX = _x + dir.dx;
            int newY = _y + dir.dy;

            if (map.IsWithinBounds(newX, newY) && !map.GetTile(newX, newY).IsWall() &&
                map.GetTile(newX, newY).GetEnemies().Count == 0)
            {
                validMoves.Add((newX, newY));
            }
        }

        if (validMoves.Count > 0)
        {
            var move = validMoves[random.Next(validMoves.Count)];
            map.GetTile(_x, _y).GetEnemies().Remove(this);
            _x = move.x;
            _y = move.y;
            map.GetTile(_x, _y).AddEnemy(this);
        }
    }

}