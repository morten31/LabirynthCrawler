namespace LabirynthCrawler.Model.Enemies;

public abstract class BaseEnemy : IEnemy
{
    protected int _x, _y;
    protected string _name;
    protected int _health;
    protected int _maxHealth;
    protected int _damage;
    protected int _defence;

    protected BaseEnemy(string name, int health, int damage, int defence)
    {
        _name = name;
        _health = health;
        _maxHealth = health;
        _damage = damage;
        _defence = defence;
    }

    public int GetX() => _x;
    public int GetY() => _y;
    
    public virtual string GetSymbol() => _name[0].ToString();
    public override string ToString() => _name;
    
    public int GetDamage() => _damage;
    public int GetHealth() => _health;
    public int GetDefence() => _defence;
    
    public void DecreaseHealth(int damage)
    {
        int actualDamage = Math.Max(0, damage - _defence);
        _health -= actualDamage;
        if (_health < 0) _health = 0;
    }

    public int GetMaxHealth() => _maxHealth;
    
    public void SetPosition(int x, int y) 
    { 
        _x = x; 
        _y = y; 
    }
    
}