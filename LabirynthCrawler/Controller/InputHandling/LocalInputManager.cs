using LabirynthCrawler.Model.Network;
using LabirynthCrawler.Model.PlayerModel;

namespace LabirynthCrawler.Controller.InputHandling;

public class LocalInputManager
{
    private enum InputState { Normal, AwaitingDropSlot, AwaitingEquipHand, AwaitingEquipSlot }
    
    private InputState _state = InputState.Normal;
    private HandSlot _pendingHand;

    public PlayerActionDto? ProcessInput(ConsoleKeyInfo key, int playerId, string currentState)
    {
        var action = new PlayerActionDto { PlayerId = playerId };

        if (currentState == "ViewingLog")
        {
            if (key.Key == ConsoleKey.Escape || KeyBindings.Matches(key, GameAction.ToggleLog))
            {
                action.ActionType = "Escape";
                return action;
            }
            return null;
        }

        if (key.Key == ConsoleKey.Escape && _state != InputState.Normal)
        {
            _state = InputState.Normal;
            return null;
        }

        switch (_state)
        {
            case InputState.Normal:
                if (KeyBindings.Matches(key, GameAction.Quit)) { action.ActionType = "Quit"; return action; }
                if (KeyBindings.Matches(key, GameAction.ToggleLog)) { action.ActionType = "ToggleLog"; return action; }

                if (KeyBindings.Matches(key, GameAction.MoveUp)) { action.ActionType = "Move"; action.Direction = "Up"; return action; }
                if (KeyBindings.Matches(key, GameAction.MoveDown)) { action.ActionType = "Move"; action.Direction = "Down"; return action; }
                if (KeyBindings.Matches(key, GameAction.MoveLeft)) { action.ActionType = "Move"; action.Direction = "Left"; return action; }
                if (KeyBindings.Matches(key, GameAction.MoveRight)) { action.ActionType = "Move"; action.Direction = "Right"; return action; }

                if (KeyBindings.Matches(key, GameAction.PickUp)) { action.ActionType = "PickUp"; return action; }

                if (KeyBindings.Matches(key, GameAction.AttackNormal)) { action.ActionType = "Attack"; action.TargetIndex = 1; return action; }
                if (KeyBindings.Matches(key, GameAction.AttackStealth)) { action.ActionType = "Attack"; action.TargetIndex = 2; return action; }
                if (KeyBindings.Matches(key, GameAction.AttackMagic)) { action.ActionType = "Attack"; action.TargetIndex = 3; return action; }

                if (KeyBindings.Matches(key, GameAction.Drop))
                {
                    _state = InputState.AwaitingDropSlot;
                    return null;
                }
                if (KeyBindings.Matches(key, GameAction.EquipLeft))
                {
                    _pendingHand = HandSlot.Left;
                    _state = InputState.AwaitingEquipSlot;
                    return null;
                }
                if (KeyBindings.Matches(key, GameAction.EquipRight))
                {
                    _pendingHand = HandSlot.Right;
                    _state = InputState.AwaitingEquipSlot;
                    return null;
                }
                
                action.ActionType = "WrongInput";
                action.Direction = key.KeyChar.ToString();
                return action;

            case InputState.AwaitingDropSlot:
                if (char.IsDigit(key.KeyChar))
                {
                    action.ActionType = "Drop";
                    action.TargetIndex = int.Parse(key.KeyChar.ToString());
                    _state = InputState.Normal;
                    return action;
                }
                _state = InputState.Normal;
                return null;

            case InputState.AwaitingEquipSlot:
                if (char.IsDigit(key.KeyChar))
                {
                    action.ActionType = "Equip";
                    action.Hand = _pendingHand;
                    action.TargetIndex = int.Parse(key.KeyChar.ToString());
                    _state = InputState.Normal;
                    return action;
                }
                _state = InputState.Normal;
                return null;
        }

        return null;
    }
}