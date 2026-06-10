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
    
    public void Render(GameStateDto dto, int localPlayerId)
    {
        if (!dto.Players.ContainsKey(localPlayerId)) return;
        PlayerDto player = dto.Players[localPlayerId];

        if (dto.CurrentState == "ViewingLog")
        {
            if (_previousStateString != "ViewingLog")
            {
                RenderLogScreenHelper(dto.RecentLogs);
                _previousStateString = "ViewingLog";
            }
            return;
        }

        if (_previousStateString == "ViewingLog")
            Console.Clear();

        DrawAttributes(player);
        DrawMap(dto);
        DrawInventory(player);
        DrawInfoBar(dto);
        DrawActions(dto);

        if (player.IsDead)
        {
            DrawDeathScreen(dto.MapHeight);
        }
        else if (dto.CurrentState == "GameOver")
        {
            DrawGameOverScreen(dto.MapHeight);
        }

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

    private void DrawMap(GameStateDto dto)
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
        
        foreach (var tile in dto.Tiles)
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

    private void DrawInfoBar(GameStateDto dto)
    {
        int infoY = BottomPanelY - 2;
        WriteAt(new string(' ', 110), LeftPanelX, infoY);
        WriteAt(">>> RECENT LOGS:", LeftPanelX, infoY);
        for (int i = 1; i <= ShortLogCount; i++) WriteAt(new string(' ', 110), LeftPanelX, infoY + i);

        for (int i = 0; i < dto.RecentLogs.Count; i++)
        {
            string logText = dto.RecentLogs[i];
            if (logText.Length > 65) logText = logText.Substring(0, 62) + "...";
            WriteAt(new string(' ', MaxRenderX), LeftPanelX, infoY + 1 + i);
            WriteAt(logText, LeftPanelX, infoY + 1 + i);
        }
    }

    private void DrawActions(GameStateDto dto)
    {
        int y = ActionsStartY;
        WriteAt("=== ACTIONS ===".PadRight(30), RightPanelX, y++);
        foreach (string msg in dto.ActionMessages)
            WriteAt(msg.TrimEnd(',', ' ').PadRight(30), RightPanelX, y++);
    }
}