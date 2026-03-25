namespace LabirynthCrawler.Model.MapGeneration;

public class InstructionBuilder : IMapBuilder
{
    private bool _isMovementMessage;
    private bool _isItemPresentMessage;
    private bool _isQuitMessage;
    private bool _isContinueMessage;

    public List<string> GetCompleteMessages()
    {
        List<string> finalMessages = new List<string>();

        if (_isMovementMessage)
            finalMessages.Add(_instructionMessages["Move"]);
        if (_isItemPresentMessage)
        {
            finalMessages.Add(_instructionMessages["PickUp"]);
            finalMessages.Add(_instructionMessages["Drop"]);
            finalMessages.Add(_instructionMessages["EquipLeft"]);
            finalMessages.Add(_instructionMessages["EquipRight"]);
        }
        if (_isQuitMessage)
            finalMessages.Add(_instructionMessages["Quit"]);
        
        return finalMessages;
    }
    
    private readonly Dictionary<string, string> _instructionMessages = new Dictionary<string, string>
        {
            { "Move", "(W/A/S/D) Move" },
            { "PickUp", "(E) Pick up item" },
            { "Drop", "(Q)[1-9] Drop item" },
            { "EquipLeft", "(K)[1-9] Equip item in left hand" },
            { "EquipRight", "(L)[1-9] Equip item in right hand" },
            { "Quit", "(ESC) Quit" }
        };

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
}