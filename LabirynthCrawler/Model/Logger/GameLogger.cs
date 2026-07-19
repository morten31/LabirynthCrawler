using System.Collections.Concurrent;

namespace LabirynthCrawler.Model.Logger;

public class LogEntry 
{
    public DateTime Timestamp { get; set; }
    public string RawMessage { get; set; }
    public int? TargetPlayerId { get; set; }
    public LogLevel Level { get; set; }
}

public class GameLogger
{
    private static GameLogger? _instance;
    public static GameLogger Instance => _instance ??= new GameLogger();
    private readonly List<LogEntry> _logs = new();
    private ILogWriter? _writer;
    public ConcurrentQueue<LogEntry> NetworkEventQueue { get; } = new();

    private GameLogger() { }

    public void SetWriter(ILogWriter writer)
    {
        _writer = writer;
    }

    public void Log(string message, LogLevel level = LogLevel.Info, int? playerId = null)
    {
        var entry = new LogEntry 
        { 
            Timestamp = DateTime.Now,
            RawMessage = message, 
            TargetPlayerId = playerId,
            Level = level
        };
        _logs.Add(entry);
        NetworkEventQueue.Enqueue(entry); 

        if (level != LogLevel.Trace) 
        {
            _writer?.Write(entry);
        }
    }

    public IReadOnlyList<LogEntry> GetAllLogs(int? forPlayerId = null)
    {
        return _logs
            .Where(l => l.TargetPlayerId == null || l.TargetPlayerId == forPlayerId)
            .ToList().AsReadOnly();
    }
    
    public IReadOnlyList<LogEntry> GetRecentLogs(int count, int? forPlayerId = null)
    {
        return _logs
            .Where(l => l.TargetPlayerId == null || l.TargetPlayerId == forPlayerId)
            .TakeLast(count)
            .ToList().AsReadOnly();
    }

    public string GetLogFileName()
    {
        return _writer?.GetLogFileName() ?? "Log file missing";
    }
}