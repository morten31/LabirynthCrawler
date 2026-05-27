namespace LabirynthCrawler.Model.Logger;

public class LogEntry 
{
    public string Text { get; set; }
    public int? TargetPlayerId { get; set; }
}

public class GameLogger
{
    private static GameLogger? _instance;
    public static GameLogger Instance => _instance ??= new GameLogger();

    private readonly List<LogEntry> _logs = new();

    private ILogWriter? _writer;

    private GameLogger() { }

    public void SetWriter(ILogWriter writer)
    {
        _writer = writer;
    }

    public void Log(string message, LogLevel level = LogLevel.Info, int? playerId = null)
    {
        string prefix = playerId.HasValue ? $"[P{playerId}] " : "[SERVER] ";
        string logText = $"[{DateTime.Now:HH:mm:ss}] {prefix}{message}";
    
        _logs.Add(new LogEntry { Text = logText, TargetPlayerId = playerId });
    
        if (level != LogLevel.Trace) _writer?.Write(logText);
    }

    public IReadOnlyList<string> GetAllLogs(int? forPlayerId = null)
    {
        return _logs
            .Where(l => l.TargetPlayerId == null || l.TargetPlayerId == forPlayerId)
            .Select(l => l.Text)
            .ToList().AsReadOnly();
    }
    public IReadOnlyList<string> GetRecentLogs(int count, int? forPlayerId = null)
    {
        return _logs.Where(l => l.TargetPlayerId == null || l.TargetPlayerId == forPlayerId)
                    .Select(l => l.Text)
                    .TakeLast(count)
                    .ToList().AsReadOnly();
    }

    public string GetLogFileName()
    {
        return _writer?.GetLogFileName() ?? "Log file missing";
    }
}