using System.Text.Json;

namespace LabirynthCrawler.Model;

public class GameConfig
{
    public string PlayerName { get; set; } = "Hero";
    public string LogDirectory { get; set; } = "./Logs";
    public string Theme { get; set; } = "Library";

    public static GameConfig LoadConfig(string path = "config.json")
    {
        if (!File.Exists(path))
        {
            var defaultConfig = new GameConfig();
            File.WriteAllText(path, JsonSerializer.Serialize(defaultConfig, new JsonSerializerOptions { WriteIndented = true }));
            return defaultConfig;
        }

        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<GameConfig>(json) ?? new GameConfig();
    }
}