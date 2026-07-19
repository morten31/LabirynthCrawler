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

public class Emerald : QuestItem { public Emerald() : base("Emerald") { } }
public class EnderPearl : QuestItem { public EnderPearl() : base("Ender Pearl") { } }
public class ChorusFruit : QuestItem { public ChorusFruit() : base("Chorus Fruit") { } }
public class GoldNugget : QuestItem { public GoldNugget() : base("Gold Nugget") { } }
public class NetherQuartz : QuestItem { public NetherQuartz() : base("Nether Quartz") { } }
