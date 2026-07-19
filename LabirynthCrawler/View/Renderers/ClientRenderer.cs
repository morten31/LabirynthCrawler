using LabirynthCrawler.Model.Logger;
using LabirynthCrawler.Model.Network;

namespace LabirynthCrawler.View.Renderers;

public class ClientRenderer : ConsoleRendererBase
{
    private WelcomeDto _welcomeData;
    private List<(int X, int Y)> _previousDynamicTiles = new();
    private bool _mapDrawn = false;
    
    public void Initialize(WelcomeDto welcome)
    {
        base.Initialize();
        _welcomeData = welcome;
    }
    
    public void Render(UpdateDto dto, List<LogEntry> localLogs, int localPlayerId, bool isViewingLog)
    {
        if (!dto.Players.ContainsKey(localPlayerId)) return;
        PlayerDto player = dto.Players[localPlayerId];
        
        if (isViewingLog) 
        {
            if (_previousStateString != "ViewingLog")
            {
                var logStrings = localLogs.Select(l => $"[{l.Timestamp:HH:mm:ss}] {l.RawMessage}").ToList();
                RenderLogScreenHelper(logStrings);
                _previousStateString = "ViewingLog";
            }
            return;
        }

        if (_previousStateString == "ViewingLog")
            Console.Clear();

        DrawAttributes(player);
        DrawMap(dto);
        DrawInventory(player);
        DrawInfoBar(localLogs);
        DrawActions();

        if (player.IsDead)
            DrawDeathScreen(_welcomeData.MapHeight);
        
        else if (dto.CurrentState == "GameOver")
            DrawGameOverScreen(_welcomeData.MapHeight);


        Console.SetCursorPosition(0, 0);
        _previousStateString = dto.CurrentState;
    }

    private void DrawAttributes(PlayerDto player)
    {
        int y = MapY;
        WriteAt("=== ATTRIBUTES ===", LeftPanelX, y++);
        WriteAt($"Health:     {player.Health}".PadRight(20), LeftPanelX, y++);
        WriteAt($"Power:      {player.Power}".PadRight(20), LeftPanelX, y++);
        WriteAt($"Agility:    {player.Agility}".PadRight(20), LeftPanelX, y++);
        WriteAt($"Luck:       {player.Luck}".PadRight(20), LeftPanelX, y++);
        WriteAt($"Aggression: {player.Aggression}".PadRight(20), LeftPanelX, y++);
        WriteAt($"Wisdom:     {player.Wisdom}".PadRight(20), LeftPanelX, y++);

        y++;
        WriteAt("=== CURRENCY ===", LeftPanelX, y++);
        WriteAt($"Coins: {player.Coins}".PadRight(20), LeftPanelX, y++);
        WriteAt($"Gold:  {player.Gold}".PadRight(20), LeftPanelX, y++);
    }

    private void DrawMap(UpdateDto dto)
    {
        if (!_mapDrawn || _previousStateString == "ViewingLog")
        {
            WriteAt("+" + new string('-', _welcomeData.MapWidth) + "+", MapX - 1, MapY - 1);
            for (int mapY = 0; mapY < _welcomeData.MapHeight; mapY++) 
                WriteAt($"|{new string(' ', _welcomeData.MapWidth)}|", MapX - 1, MapY + mapY);
            WriteAt("+" + new string('-', _welcomeData.MapWidth) + "+", MapX - 1, MapY + _welcomeData.MapHeight);
            
            Console.ForegroundColor = ConsoleColor.DarkGray;
            foreach (var wall in _welcomeData.StaticWalls)
                WriteAt(wall.Symbol, MapX + wall.X, MapY + wall.Y);
            Console.ResetColor();

            _mapDrawn = true;
        }
        
        foreach (var pos in _previousDynamicTiles)
        {
            WriteAt(" ", MapX + pos.X, MapY + pos.Y);
        }
        _previousDynamicTiles.Clear();
        
        foreach (var tile in dto.DynamicTiles)
        {
            WriteAt(tile.Symbol, MapX + tile.X, MapY + tile.Y);
            _previousDynamicTiles.Add((tile.X, tile.Y));
        }
        
        
        foreach (var kvp in dto.Players)
        {
            Console.SetCursorPosition(MapX + kvp.Value.X, MapY + kvp.Value.Y);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(kvp.Key.ToString());
            Console.ResetColor();
            _previousDynamicTiles.Add((kvp.Value.X, kvp.Value.Y));
        }
    }

    private void DrawInventory(PlayerDto player)
    {
        int y = MapY;
        WriteAt("=== EQUIPPED ===", RightPanelX, y++);
        WriteAt($"Left Hand:  {player.LeftHand}".PadRight(52), RightPanelX, y++);
        WriteAt($"Right Hand: {player.RightHand}".PadRight(52), RightPanelX, y++);

        y++;
        WriteAt("=== INVENTORY ===", RightPanelX, y++);
        for (int i = 0; i < 10; i++) WriteAt(new string(' ', 30), RightPanelX, y + i);
        
        for (int i = 0; i < player.Inventory.Count; i++)
            WriteAt($"{i + 1}. {player.Inventory[i]}", RightPanelX, y++);
    }

    private void DrawInfoBar(List<LogEntry> localLogs)
    {
        int infoY = BottomPanelY - 2;
        WriteAt(new string(' ', 110), LeftPanelX, infoY);
        WriteAt(">>> RECENT LOGS:", LeftPanelX, infoY);
        for (int i = 1; i <= ShortLogCount; i++) WriteAt(new string(' ', 110), LeftPanelX, infoY + i);

        var recentLogs = localLogs.TakeLast(5).ToList();
        for (int i = 0; i < recentLogs.Count; i++)
        {
            string logText = $"[{recentLogs[i].Timestamp:HH:mm:ss}] {recentLogs[i].RawMessage}";
            if (logText.Length > 65) logText = logText.Substring(0, 62) + "...";
            WriteAt(new string(' ', MaxRenderX), LeftPanelX, infoY + 1 + i);
            WriteAt(logText, LeftPanelX, infoY + 1 + i);
        }
    }

    private void DrawActions()
    {
        int y = ActionsStartY;
        foreach (string msg in StaticActionMessages)
        {
            WriteAt(msg.PadRight(30), RightPanelX, y++);
        }
    }
}