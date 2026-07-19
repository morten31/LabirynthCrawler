namespace LabirynthCrawler.Model.Logger;

public enum LogLevel
{
    Trace,
    Info,
    Combat,
    System
}

public interface ILogWriter
{
    void Write(LogEntry entry);
    string GetLogFileName();
}

public class FileLogWriter : ILogWriter
{
    private readonly string _filePath;
    private readonly string _fileName;
    
    public FileLogWriter(string playerName, string directory)
    {
        string fullDirPath = Path.GetFullPath(directory); 
        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        _fileName = $"log_{playerName}_{timestamp}.txt";
        _filePath = Path.Combine(fullDirPath, _fileName);
    }

    public void Write(LogEntry entry)
    {
        string prefix = entry.TargetPlayerId.HasValue ? $"[P{entry.TargetPlayerId}] " : "[SERVER] ";
        string logText = $"[{entry.Timestamp:HH:mm:ss}] {prefix}{entry.RawMessage}";
        File.AppendAllText(_filePath, logText + Environment.NewLine);    }
    
    public string GetLogFileName() => _fileName;
    
}

public class DummyWriter : ILogWriter
{
    public void Write(LogEntry entry) { }
    public string GetLogFileName() => string.Empty;
}