using LabirynthCrawler.Model;
using LabirynthCrawler.Model.Board;
using LabirynthCrawler.Model.Logger;
using LabirynthCrawler.Model.PlayerModel;

namespace LabirynthCrawler.View.Renderers;

public class LocalRenderer : ConsoleRendererBase
{
    public void Render(GameModel model, int localPlayerId)
    {
        lock (model.StateLock)
        {
            Player? player = model.GetPlayer(localPlayerId);
            if (player == null) return;

            if (player.IsViewingLog) 
            {
                if (_previousStateString != "ViewingLog")
                {
                    RenderLogScreenHelper(GameLogger.Instance.GetAllLogs(localPlayerId));
                    _previousStateString = "ViewingLog";
                }
                return;
            }

            if (_previousStateString == "ViewingLog")
                Console.Clear();

            DrawAttributes(player);
            DrawMap(model);
            DrawInventory(player);
            DrawInfoBar(model, player);
            DrawActions(model);

            if (player.IsDead)
            {
                DrawDeathScreen(Map.Height);
                
                string logFile = GameLogger.Instance.GetLogFileName();
                string logMsg = $"Log saved at: {logFile}";
                Console.ForegroundColor = ConsoleColor.Yellow;
                WriteAt(logMsg, MapX + 5 - (logMsg.Length/2) + 14, MapY + Map.Height / 2 + 5);
                Console.ResetColor();
            }
            else if (model.CurrentState == GameModel.GameState.GameOver)
            {
                DrawGameOverScreen(Map.Height);
            }

            Console.SetCursorPosition(0, 0);
            _previousState = model.CurrentState;
            
            _previousStateString = "Playing"; 
        }
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

        foreach (var kvp in model.Players)
        {
            Console.SetCursorPosition(MapX + kvp.Value.GetX(), MapY + kvp.Value.GetY());
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(kvp.Key.ToString());
            Console.ResetColor();
        }
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
        for (int i = 0; i < 10; i++) WriteAt(new string(' ', 30), RightPanelX, y + i);

        var items = inv.GetItems();
        for (int i = 0; i < items.Count; i++)
            WriteAt($"{i + 1}. {items[i].ToString()}", RightPanelX, y++);
    }

    private void DrawInfoBar(GameModel model, Player player)
    {
        int infoY = BottomPanelY - 2;
        WriteAt(new string(' ', 110), LeftPanelX, infoY);
        WriteAt(">>> RECENT LOGS:", LeftPanelX, infoY);
        for (int i = 1; i <= ShortLogCount; i++) WriteAt(new string(' ', 110), LeftPanelX, infoY + i);

        var recentLogs = GameLogger.Instance.GetRecentLogs(5);
        for (int i = 0; i < recentLogs.Count; i++)
        {
            string logText = recentLogs[i];
            if (logText.Length > 65) logText = logText.Substring(0, 62) + "...";
            WriteAt(new string(' ', MaxRenderX), LeftPanelX, infoY + 1 + i);
            WriteAt(logText, LeftPanelX, infoY + 1 + i);
        }
    }

    private void DrawActions(GameModel model)
    {
        var messages = model.GetInstructionBuilder().GetCompleteMessages();
        int y = ActionsStartY;
        WriteAt("=== ACTIONS ===".PadRight(30), RightPanelX, y++);
        foreach (string msg in messages)
            WriteAt(msg.TrimEnd(',', ' ').PadRight(30), RightPanelX, y++);
    }
}