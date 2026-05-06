namespace LabirynthCrawler.Model.Observers;

using System.Collections.Generic;

public class SpeciesFaction : ISpeciesPublisher
{
    private readonly List<IDeathObserver> _observers = new List<IDeathObserver>();

    public void Attach(IDeathObserver observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }
    }

    public void Detach(IDeathObserver observer)
    {
        _observers.Remove(observer);
    }

    public void NotifyDeath()
    {
        var currentObservers = new List<IDeathObserver>(_observers);
        
        foreach (var observer in currentObservers)
        {
            observer.OnAllyDeath();
        }
    }
}