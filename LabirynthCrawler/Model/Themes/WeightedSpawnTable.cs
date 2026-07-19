namespace LabirynthCrawler.Model.Themes;

public class WeightedSpawnTable<T>
{
    private readonly List<(Func<T> FactoryMethod, int Weight)> _items = new();
    private int _totalWeight = 0;
    private static readonly Random _rng = new Random();

    public void AddEntry(Func<T> factoryMethod, int weight)
    {
        _items.Add((factoryMethod, weight));
        _totalWeight += weight;
    }

    public T Spawn()
    {
        if (_totalWeight == 0) throw new InvalidOperationException("Loot table is empty!");
        
        int roll = _rng.Next(_totalWeight);
        foreach (var item in _items)
        {
            if (roll < item.Weight)
                return item.FactoryMethod();
            roll -= item.Weight;
        }
        return _items.Last().FactoryMethod();
    }
}