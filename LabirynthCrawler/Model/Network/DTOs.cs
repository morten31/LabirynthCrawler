using System.Text.Json.Serialization;
using LabirynthCrawler.Model.Logger;
using LabirynthCrawler.Model.PlayerModel;

namespace LabirynthCrawler.Model.Network;

public class GameStateDto
{
    public int MapWidth { get; set; }
    public int MapHeight { get; set; }
    public List<TileDto> Tiles { get; set; } = new();
    public Dictionary<int, PlayerDto> Players { get; set; } = new();
    public List<string> RecentLogs { get; set; } = new();
    public string CurrentState { get; set; } = "Playing";
}

public class TileDto
{
    public int X { get; set; }
    public int Y { get; set; }
    public string Symbol { get; set; } = " ";
    public bool IsWall { get; set; }
}

public class PlayerDto
{
    public int Id { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public bool IsDead { get; set; }

    public int Health { get; set; }
    public int Power { get; set; }
    public int Agility { get; set; }
    public int Luck { get; set; }
    public int Aggression { get; set; }
    public int Wisdom { get; set; }

    public int Coins { get; set; }
    public int Gold { get; set; }

    public List<string> Inventory { get; set; } = new();
    public string LeftHand { get; set; } = "Empty";
    public string RightHand { get; set; } = "Empty";
    
    public bool IsViewingLog { get; set; }

}

public class PlayerActionDto
{
    public int PlayerId { get; set; }
    public string ActionType { get; set; } = ""; // "Move", "PickUp", "Drop", "Equip", "Attack"
    public string Direction { get; set; } = ""; // "Up", "Down", "Left", "Right"
    public int TargetIndex { get; set; }
    public HandSlot Hand { get; set; }
}

public class WelcomeDto
{
    public int AssignedPlayerId { get; set; }
    public int MapWidth { get; set; }
    public int MapHeight { get; set; }
    public List<TileDto> StaticWalls { get; set; } = new();
}

public class UpdateDto
{
    public Dictionary<int, PlayerDto> Players { get; set; } = new();
    public List<TileDto> DynamicTiles { get; set; } = new();
    public List<LogEntry> NewEvents { get; set; } = new();
    public string CurrentState { get; set; } = "Playing";
}