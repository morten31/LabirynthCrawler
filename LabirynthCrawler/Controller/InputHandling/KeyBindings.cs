namespace LabirynthCrawler.Controller.InputHandling;

public enum GameAction
{
    MoveUp,
    MoveDown,
    MoveLeft,
    MoveRight,
    PickUp,
    Drop,
    EquipLeft,
    EquipRight,
    AttackNormal,
    AttackStealth,
    AttackMagic,    
    Quit
}

public static class KeyBindings
{
    public static readonly Dictionary<GameAction, (ConsoleKey Key, string Description)> Bindings = new()
    {
        { GameAction.MoveUp,    (ConsoleKey.W, "Move Up") },
        { GameAction.MoveLeft,  (ConsoleKey.A, "Move Left") },
        { GameAction.MoveDown,  (ConsoleKey.S, "Move Down") },
        { GameAction.MoveRight, (ConsoleKey.D, "Move Right") },
        
        { GameAction.PickUp,    (ConsoleKey.E, "Pick up item") },
        { GameAction.Drop,      (ConsoleKey.Q, "[1-9] Drop item") },
        
        { GameAction.EquipLeft, (ConsoleKey.K, "[1-9] Equip Left") },
        { GameAction.EquipRight,(ConsoleKey.L, "[1-9] Equip Right") },
        
        { GameAction.AttackNormal,  (ConsoleKey.B, "Normal Attack") },
        { GameAction.AttackStealth, (ConsoleKey.N, "Stealth Attack") },
        { GameAction.AttackMagic,   (ConsoleKey.M, "Magic Attack") },
        
        { GameAction.Quit,      (ConsoleKey.Escape, "Quit Game") }
    };
    
    public static bool Matches(ConsoleKeyInfo keyInfo, GameAction action)
    {
        return Bindings.TryGetValue(action, out var mapping) && keyInfo.Key == mapping.Key;
    }
}