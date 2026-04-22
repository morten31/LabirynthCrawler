namespace LabirynthCrawler.Model.Logger;

public class GameLogger
{
    private static GameLogger? _instance;
    public static GameLogger Instance => _instance ??= new GameLogger();

    private readonly List<string> _logs = new();

    private ILogWriter? _writer;

    private GameLogger()
    {
    }

    public void SetWriter(ILogWriter writer)
    {
        _writer = writer;
    }

    public void Log(string message)
    {
        string logEntry = $"[{DateTime.Now:HH:mm:ss}] {message}";

        _logs.Add(message);

        _writer?.Write(logEntry);
    }

    public IReadOnlyList<string> GetAllLogs() => _logs.AsReadOnly();

    public IReadOnlyList<string> GetRecentLogs(int count)
    {
        return _logs.Skip(Math.Max(0, _logs.Count - count)).Reverse().ToList().AsReadOnly();
    }

    public string GetLogFileName()
    {
        return _writer?.GetLogFileName() ?? "Log file missing";
    }
}