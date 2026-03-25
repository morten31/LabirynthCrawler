namespace LabirynthCrawler.Model.Items;

public abstract class QuestItem : ItemBase
{
    public override char GetSymbol() => 'Q';
    
    public QuestItem(string name)
    {
        _name = name;
    }
}

public class DungeonKey : QuestItem
{
    public DungeonKey() : base("Dungeon Key") { }
}

public class TrollSkull : QuestItem
{
    public TrollSkull() : base("Troll's skull") { }
}

public class DragonEgg : QuestItem
{
    public DragonEgg() : base("Dragon's Egg") { }
}