using LabirynthCrawler.Model.Network;

namespace LabirynthCrawler.Controller.InputHandling;

public static class InputParser
{
    public static PlayerActionDto? ParseInput(int playerId, string currentState)
    {
        if (!Console.KeyAvailable) return null;
        ConsoleKeyInfo key = Console.ReadKey(true);
        var action = new PlayerActionDto { PlayerId = playerId };

        if (KeyBindings.Matches(key, GameAction.Quit)) { action.ActionType = "Quit"; return action; }
        
        if (currentState == "ViewingLog" && key.Key == ConsoleKey.Escape) 
            { action.ActionType = "Escape"; return action; }

        if (KeyBindings.Matches(key, GameAction.ToggleLog)) { action.ActionType = "ToggleLog"; return action; }

        if (KeyBindings.Matches(key, GameAction.MoveUp)) { action.ActionType = "Move"; action.Direction = "Up"; return action; }
        if (KeyBindings.Matches(key, GameAction.MoveDown)) { action.ActionType = "Move"; action.Direction = "Down"; return action; }
        if (KeyBindings.Matches(key, GameAction.MoveLeft)) { action.ActionType = "Move"; action.Direction = "Left"; return action; }
        if (KeyBindings.Matches(key, GameAction.MoveRight)) { action.ActionType = "Move"; action.Direction = "Right"; return action; }

        if (KeyBindings.Matches(key, GameAction.PickUp)) { action.ActionType = "PickUp"; return action; }

        if (KeyBindings.Matches(key, GameAction.Drop))
        {
            var k2 = Console.ReadKey(true);
            if (char.IsDigit(k2.KeyChar))
            {
                action.ActionType = "Drop";
                action.TargetIndex = int.Parse(k2.KeyChar.ToString());
                return action;
            }
        }

        bool isLeft = KeyBindings.Matches(key, GameAction.EquipLeft);
        bool isRight = KeyBindings.Matches(key, GameAction.EquipRight);
        if (isLeft || isRight)
        {
            var k2 = Console.ReadKey(true);
            if (char.IsDigit(k2.KeyChar))
            {
                action.ActionType = "Equip";
                action.Hand = isLeft ? 'L' : 'R';
                action.TargetIndex = int.Parse(k2.KeyChar.ToString());
                return action;
            }
        }

        if (KeyBindings.Matches(key, GameAction.AttackNormal)) { action.ActionType = "Attack"; action.TargetIndex = 1; return action; }
        if (KeyBindings.Matches(key, GameAction.AttackStealth)) { action.ActionType = "Attack"; action.TargetIndex = 2; return action; }
        if (KeyBindings.Matches(key, GameAction.AttackMagic)) { action.ActionType = "Attack"; action.TargetIndex = 3; return action; }

        action.ActionType = "WrongInput";
        action.Direction = key.KeyChar.ToString();
        return action;
    }
}