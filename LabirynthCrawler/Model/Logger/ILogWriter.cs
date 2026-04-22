namespace LabirynthCrawler.Model.Logger;

public interface ILogWriter
{
    void Write(string message);
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

    public void Write(string message)
    {
        File.AppendAllText(_filePath, message + Environment.NewLine);
    }
    
    public string GetLogFileName() => _fileName;
    
}

public class DummyWriter : ILogWriter
{
    public void Write(string message) { }
    public string GetLogFileName() => string.Empty;
}