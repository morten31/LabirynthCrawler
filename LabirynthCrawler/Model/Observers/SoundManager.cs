using LabirynthCrawler.Model.Board;
using LabirynthCrawler.Model.Enemies;
using LabirynthCrawler.Model.Logger;

namespace LabirynthCrawler.Model.Observers;

public class SoundManager : ISoundPublisher
{
    private readonly List<ISoundObserver> _observers = new List<ISoundObserver>();
    private Map? _map;

    public void Initialize(Map map)
    {
        _map = map;
    }

    public void Attach(ISoundObserver observer)
    {
        if (!_observers.Contains(observer))
            _observers.Add(observer);
    }

    public void Detach(ISoundObserver observer)
    {
        _observers.Remove(observer);
    }

    public void NotifySound(int sourceX, int sourceY, int range, string sourceName)
    {
        if (_map == null) return;

        var observersAtLocation = new Dictionary<(int x, int y), List<ISoundObserver>>();
        foreach (var observer in _observers)
        {
            var pos = (observer.GetX(), observer.GetY());
            if (!observersAtLocation.ContainsKey(pos))
            {
                observersAtLocation[pos] = new List<ISoundObserver>();
            }
            observersAtLocation[pos].Add(observer);
        }
        
        var queue = new Queue<(int x, int y, int dist)>();
        
        var visited = new HashSet<(int x, int y)>();
        
        queue.Enqueue((sourceX, sourceY, 0));
        visited.Add((sourceX, sourceY));
        int[] dx = { 0, 0, 1, -1 };
        int[] dy = { 1, -1, 0, 0 };
        
        while (queue.Count > 0)
        {
            var (currX, currY, dist) = queue.Dequeue();

            if (observersAtLocation.TryGetValue((currX, currY), out var listeners))
            {
                foreach (var listener in listeners)
                {
                    listener.OnSoundHeard(sourceX, sourceY, dist, sourceName);
                }
            }
            if (dist < range)
            {
                for (int i = 0; i < 4; i++)
                {
                    int nextX = currX + dx[i];
                    int nextY = currY + dy[i];
                    var nextPos = (nextX, nextY);

                    if (_map.IsWithinBounds(nextX, nextY) &&
                        !_map.GetTile(nextX, nextY).IsWall() &&
                        !visited.Contains(nextPos))
                    {
                        visited.Add(nextPos);
                        queue.Enqueue((nextX, nextY, dist + 1));
                    }
                }
            }
        }
    }

    private bool IsWallBetween(int startX, int startY, int endX, int endY)
    {
        int dx = Math.Sign(endX - startX);
        int dy = Math.Sign(endY - startY);

        int currX = startX + dx;
        int currY = startY + dy;

        while (currX != endX || currY != endY)
        {
            if (_map!.IsWithinBounds(currX, currY) && _map.GetTile(currX, currY).IsWall())
            {
                return true;
            }
            if (currX != endX) currX += dx;
            if (currY != endY) currY += dy;
        }

        return false;
    }
}