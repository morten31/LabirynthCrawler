using LabirynthCrawler.Controller.InputHandling;

namespace LabirynthCrawler.Model.MapGeneration;

public class InstructionBuilder : IMapBuilder
{
    private bool _isMovementMessage;
    private bool _isItemPresentMessage;
    private bool _isQuitMessage;
    private bool _isCombatMessage;

    public List<string> GetCompleteMessages()
    {
        List<string> finalMessages = new List<string>();

        if (_isMovementMessage)
        {
            string up = KeyBindings.Bindings[GameAction.MoveUp].Key.ToString();
            string left = KeyBindings.Bindings[GameAction.MoveLeft].Key.ToString();
            string down = KeyBindings.Bindings[GameAction.MoveDown].Key.ToString();
            string right = KeyBindings.Bindings[GameAction.MoveRight].Key.ToString();
            finalMessages.Add($"({up}/{left}/{down}/{right}) Move");
        }

        if (_isItemPresentMessage)
        {
            var pickUp = KeyBindings.Bindings[GameAction.PickUp];
            var drop = KeyBindings.Bindings[GameAction.Drop];
            var equipLeft = KeyBindings.Bindings[GameAction.EquipLeft];
            var equipRight = KeyBindings.Bindings[GameAction.EquipRight];

            finalMessages.Add($"({pickUp.Key}) {pickUp.Description}");
            finalMessages.Add($"({drop.Key}) {drop.Description}");
            finalMessages.Add($"({equipLeft.Key}) {equipLeft.Description}");
            finalMessages.Add($"({equipRight.Key}) {equipRight.Description}");
        }
        
        if (_isCombatMessage)
        {
            var atkNorm = KeyBindings.Bindings[GameAction.AttackNormal];
            var atkStealth = KeyBindings.Bindings[GameAction.AttackStealth];
            var atkMagic = KeyBindings.Bindings[GameAction.AttackMagic];

            finalMessages.Add($"({atkNorm.Key}) {atkNorm.Description}");
            finalMessages.Add($"({atkStealth.Key}) {atkStealth.Description}");
            finalMessages.Add($"({atkMagic.Key}) {atkMagic.Description}");
        }
        
        if (_isQuitMessage)
        {
            var quit = KeyBindings.Bindings[GameAction.Quit];
            finalMessages.Add($"({quit.Key}) {quit.Description}");
        }

        return finalMessages;
    }

    public IMapBuilder BuildEmpty()
    {
        _isMovementMessage = true;
        _isQuitMessage = true;
        return this;
    }

    public IMapBuilder FillWithWalls()
    {
        _isMovementMessage = true;
        _isQuitMessage = true;
        return this;
    }
    public IMapBuilder AddCorridors() => this;
    public IMapBuilder AddRooms(int roomCount, int roomSize) => this;
    public IMapBuilder AddMainRoom(int roomSizeX, int roomSizeY) => this;

    public IMapBuilder AddItems(int count)
    {
        _isItemPresentMessage = true;
        return this;
    }

    public IMapBuilder AddWeapons(int count)
    {
        
        _isItemPresentMessage = true;
        return this;
    }

    public IMapBuilder AddEnemies(int count)
    {
        _isCombatMessage = true;
        return this;
    }
}