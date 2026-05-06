namespace LabirynthCrawler.Model.Observers;

public interface ISoundPublisher
{
    void Attach(ISoundObserver observer);
    void Detach(ISoundObserver observer);
    void NotifySound(int sourceX, int sourceY, int range, string sourceName);
}