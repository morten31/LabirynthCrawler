using System;
using LabirynthCrawler.Controller.InputHandling;
using LabirynthCrawler.Model;
using LabirynthCrawler.Model.PlayerModel;
using LabirynthCrawler.Model.Board;
using LabirynthCrawler.Model.Logger;

namespace LabirynthCrawler.View;

public class FrameRenderer
{
    private const int MaxRenderY = 35;
    private const int MaxRenderX = 110;
    
    private const int LeftPanelX = 0;
    
    private const int MapX = 24; 
    private const int MapY = 1; 
    
    private const int RightPanelX = 68;
    private const int BottomPanelY = 25;
    private const int ActionsStartY = 16;

    private const int ShortLogCount = 5;
    

    private GameModel.GameState _previousState = GameModel.GameState.Playing;

    public void Initialize()
    {
        Console.Clear();
        Console.CursorVisible = false;
    }

    public void Render(GameModel model)
    {
        Player player = model.GetPlayer();
        
        if (model.CurrentState == GameModel.GameState.ViewingLog)
        {
            if (_previousState != GameModel.GameState.ViewingLog)
            {
                RenderLogScreen();
                _previousState = model.CurrentState;
            }

            return;
        }

        if (_previousState == GameModel.GameState.ViewingLog)
            Console.Clear();
        
        DrawAttributes(player);
        DrawMap(model);
        DrawInventory(player);
        DrawInfoBar(model);
        DrawActions(model);
        if (model.CurrentState == GameModel.GameState.GameOver)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            WriteAt(" ============================ ", MapX + 5, MapY + Map.Height / 2 - 1);
            WriteAt(" =        YOU DIED!         = ", MapX + 5, MapY + Map.Height / 2);
            WriteAt(" =   Press ESC to leave.    = ", MapX + 5, MapY + Map.Height / 2 + 1);
            WriteAt(" ============================ ", MapX + 5, MapY + Map.Height / 2 + 2);
            
            string logFile = GameLogger.Instance.GetLogFileName();
            string logMsg = $"Log saved at: {logFile}";
            Console.ForegroundColor = ConsoleColor.Yellow;
            WriteAt(logMsg, MapX + 5 - (logMsg.Length/2) + 14, MapY + Map.Height / 2 + 4);
            
            Console.ResetColor();
        }

        Console.SetCursorPosition(0, 0);

        _previousState = model.CurrentState;
    }

    private void DrawAttributes(Player player)
    {
        Attributes attr = player.GetTotalAttributes(); 
        Inventory inv = player.GetInventory();

        int y = MapY;
        WriteAt("=== ATTRIBUTES ===", LeftPanelX, y++);
        WriteAt($"Health:     {attr.Health}".PadRight(20), LeftPanelX, y++);
        WriteAt($"Power:      {attr.Power}".PadRight(20), LeftPanelX, y++);
        WriteAt($"Agility:    {attr.Agility}".PadRight(20), LeftPanelX, y++);
        WriteAt($"Luck:       {attr.Luck}".PadRight(20), LeftPanelX, y++);
        WriteAt($"Aggression: {attr.Aggression}".PadRight(20), LeftPanelX, y++);
        WriteAt($"Wisdom:     {attr.Wisdom}".PadRight(20), LeftPanelX, y++);
    
        y++;
        WriteAt("=== CURRENCY ===", LeftPanelX, y++);
        WriteAt($"Coins: {inv.GetCoinsCount()}".PadRight(20), LeftPanelX, y++);
        WriteAt($"Gold:  {inv.GetGoldCount()}".PadRight(20), LeftPanelX, y++);
    }

    private void DrawMap(GameModel model)
    {
        Map map = model.GetMap(); 
        Player player = model.GetPlayer();

        WriteAt("+" + new string('-', Map.Width) + "+", MapX - 1, MapY - 1);

        for (int y = 0; y < Map.Height; y++)
        {
            string rowText = string.Empty;
            for (int x = 0; x < Map.Width; x++)
            {
                Tile tile = map.GetTile(x, y);
                rowText += tile.GetSymbol();
            }

            WriteAt($"|{rowText}|", MapX - 1, MapY + y);
        }

        WriteAt("+" + new string('-', Map.Width) + "+", MapX - 1, MapY + Map.Height);

        Console.SetCursorPosition(MapX + player.GetX(), MapY + player.GetY());
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write('@');
        Console.ResetColor();
    }

    private void DrawInventory(Player player)
    {
        Inventory inv = player.GetInventory();
        int y = MapY;
    
        WriteAt("=== EQUIPPED ===", RightPanelX, y++);
        var hands = inv.GetHandsContent();
    
        string leftHand = hands.Item1 != null ? hands.Item1.ToString()! : "Empty";
        string rightHand = hands.Item2 != null ? hands.Item2.ToString()! : "Empty";
    
        WriteAt($"Left Hand:  {leftHand}".PadRight(52), RightPanelX, y++);
        WriteAt($"Right Hand: {rightHand}".PadRight(52), RightPanelX, y++);
    
        y++;
        WriteAt("=== INVENTORY ===", RightPanelX, y++);
    
        for (int i = 0; i < 10; i++) 
        {
            WriteAt(new string(' ', 30), RightPanelX, y + i);
        }

        var items = inv.GetItems(); 
        for (int i = 0; i < items.Count; i++)
        {
            WriteAt($"{i+1}. {items[i].ToString()}", RightPanelX, y++);
        }
    }

    private void DrawInfoBar(GameModel model)
    {
        Player player = model.GetPlayer();
        Tile tile = model.GetMap().GetTile(player.GetX(), player.GetY());
        
        int infoY = BottomPanelY - 2;
        
        WriteAt(new string(' ', 110), LeftPanelX, infoY);
        WriteAt(">>> RECENT LOGS:", LeftPanelX, infoY);
        for (int i = 1; i <= ShortLogCount; i++)
        {
            WriteAt(new string(' ', 110), LeftPanelX, infoY + i);
        }
    
        var recentLogs = GameLogger.Instance.GetRecentLogs(5);
        for (int i = 0; i < recentLogs.Count; i++)
        {
            WriteAt(recentLogs[i], LeftPanelX, infoY + 1 + i);
        }
    }

    private void DrawActions(GameModel model)
    {
        List<string> messages = model.GetInstructionBuilder().GetCompleteMessages();

        int y = ActionsStartY;
        WriteAt("=== ACTIONS ===".PadRight(30), RightPanelX, y++);
        foreach (string msg in messages)
            WriteAt(msg.TrimEnd(',', ' ').PadRight(30), RightPanelX, y++);
    }
    
    private void RenderLogScreen()
    {
        Console.Clear();
        Console.WriteLine("================================ EVENT DIARY ================================");
        Console.WriteLine($" (Press {KeyBindings.Bindings[GameAction.ToggleLog].Key.ToString()}" +
                          $" or {KeyBindings.Bindings[GameAction.Quit].Key.ToString()}, to return to the game)\n");
        
        var logs = GameLogger.Instance.GetAllLogs();

        for (int i = 0; i < logs.Count; i++)
        {
            Console.WriteLine(logs[i]);
        }
    }
    
    

    private void WriteAt(string text, int x, int y)
    {
        Console.SetCursorPosition(x, y);
        Console.Write(text);
    }
}