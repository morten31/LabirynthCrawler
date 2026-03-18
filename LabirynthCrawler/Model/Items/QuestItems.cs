namespace LabirynthCrawler.Model.Items;

public class QuestItem : ItemBase
{
    public override char GetSymbol() => 'Q';
    
    public QuestItem(string name)
    {
        _name = name;
    }
}


// public class DungeonKey : ItemBase
// {
//     public override string GetName() => "Dungeon Key";
//     public override char GetSymbol() => "Q";
// }
//
// public class ChaosElement : ItemBase
// {
//     public override string GetName() => "Element Of Chaos";
//     public override char GetSymbol() => "Q";
// }
//
// public class DragonEgg : ItemBase
// {
//     public override string GetName() => "Dragon's Egg";
//     public override char GetSymbol() => "Q";
// }