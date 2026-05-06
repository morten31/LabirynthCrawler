namespace LabirynthCrawler.Model.Observers;

public interface ISoundObserver
{
    int GetX();
    int GetY();
    void OnSoundHeard(int sourceX, int sourceY, int distance, string sourceName);
}