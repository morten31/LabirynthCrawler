namespace LabirynthCrawler.Model.Enemies;

public interface IEnemy
{
    public int GetX();
    public int GetY();
    public string GetSymbol();
    public string? ToString();
    public int GetDamage();
    public int GetHealth();
    public int GetDefence();
    public void DecreaseHealth(int damage);
    public int GetMaxHealth();
    public void SetPosition(int x, int y);
}