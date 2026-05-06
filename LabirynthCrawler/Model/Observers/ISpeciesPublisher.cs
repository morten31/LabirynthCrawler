namespace LabirynthCrawler.Model.Observers;

public interface ISpeciesPublisher
{
    void Attach(IDeathObserver observer);
    void Detach(IDeathObserver observer);
    void NotifyDeath();
}