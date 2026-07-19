using LabirynthCrawler.Model;
using LabirynthCrawler.Controller.InputHandling;

namespace LabirynthCrawler.View.Renderers;

public abstract class ConsoleRendererBase
{
    protected const int MaxRenderY = 35;
    protected const int MaxRenderX = 110;
    
    protected const int LeftPanelX = 0;
    
    protected const int MapX = 24; 
    protected const int MapY = 1; 
    
    protected const int RightPanelX = 68;
    protected const int BottomPanelY = 25;
    protected const int ActionsStartY = 16;

    protected const int ShortLogCount = 5;

    protected GameModel.GameState _previousState = GameModel.GameState.Playing;
    protected string _previousStateString = "Playing";
    protected List<string> StaticActionMessages = new();

    public virtual void Initialize()
    {
        Console.Clear();
        Console.CursorVisible = false;
        GenerateStaticInstructions(); 
    }
    
    private void GenerateStaticInstructions()
    {
        StaticActionMessages.Add("=== ACTIONS ===");
    
        var up = KeyBindings.Bindings[GameAction.MoveUp].Key;
        var left = KeyBindings.Bindings[GameAction.MoveLeft].Key;
        var down = KeyBindings.Bindings[GameAction.MoveDown].Key;
        var right = KeyBindings.Bindings[GameAction.MoveRight].Key;
        StaticActionMessages.Add($"({up}/{left}/{down}/{right}) Move");

        var pickUp = KeyBindings.Bindings[GameAction.PickUp];
        var drop = KeyBindings.Bindings[GameAction.Drop];
        StaticActionMessages.Add($"({pickUp.Key}) {pickUp.Description}");
        StaticActionMessages.Add($"({drop.Key}) {drop.Description}");

        var eqL = KeyBindings.Bindings[GameAction.EquipLeft];
        var eqR = KeyBindings.Bindings[GameAction.EquipRight];
        StaticActionMessages.Add($"({eqL.Key}) {eqL.Description}");
        StaticActionMessages.Add($"({eqR.Key}) {eqR.Description}");

        var atkN = KeyBindings.Bindings[GameAction.AttackNormal];
        var atkS = KeyBindings.Bindings[GameAction.AttackStealth];
        var atkM = KeyBindings.Bindings[GameAction.AttackMagic];
        StaticActionMessages.Add($"({atkN.Key}) {atkN.Description}");
        StaticActionMessages.Add($"({atkS.Key}) {atkS.Description}");
        StaticActionMessages.Add($"({atkM.Key}) {atkM.Description}");
    
        var log = KeyBindings.Bindings[GameAction.ToggleLog];
        var quit = KeyBindings.Bindings[GameAction.Quit];
        StaticActionMessages.Add($"({log.Key}) {log.Description}");
        StaticActionMessages.Add($"({quit.Key}) {quit.Description}");
    }

    protected void WriteAt(string text, int x, int y)
    {
        Console.SetCursorPosition(x, y);
        Console.Write(text);
    }
    
    protected void DrawDeathScreen(int mapHeight)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        WriteAt(" ============================ ", MapX + 5, MapY + mapHeight / 2 - 1);
        WriteAt(" =        YOU DIED!         = ", MapX + 5, MapY + mapHeight / 2);
        WriteAt(" =    Spectating mode...    = ", MapX + 5, MapY + mapHeight / 2 + 1);
        WriteAt(" =   Press ESC to quit.     = ", MapX + 5, MapY + mapHeight / 2 + 2);
        WriteAt(" ============================ ", MapX + 5, MapY + mapHeight / 2 + 3);
        Console.ResetColor();
    }
    
    protected void DrawGameOverScreen(int mapHeight)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        WriteAt(" ============================ ", MapX + 5, MapY + mapHeight / 2);
        WriteAt(" =     ALL PLAYERS DEAD     = ", MapX + 5, MapY + mapHeight / 2 + 1);
        WriteAt(" ============================ ", MapX + 5, MapY + mapHeight / 2 + 2);
        Console.ResetColor();
    }
    
    protected void RenderLogScreenHelper(IReadOnlyList<string> logs)
    {
        Console.Clear();
        Console.WriteLine("================================ EVENT DIARY ================================");
        Console.WriteLine($" (Press {KeyBindings.Bindings[GameAction.ToggleLog].Key.ToString()}" +
                          $" or {KeyBindings.Bindings[GameAction.Quit].Key.ToString()}, to return to the game)\n");

        for (int i = 0; i < logs.Count; i++)
        {
            Console.WriteLine(logs[i]);
        }
    }
}