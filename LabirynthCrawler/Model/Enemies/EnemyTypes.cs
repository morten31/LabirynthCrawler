namespace LabirynthCrawler.Model.Enemies;

public class Goblin : BaseEnemy
{
    public Goblin() : base("Goblin", health: 20, damage: 8, defence: 1) { }
}

public class Orc : BaseEnemy
{
    public Orc() : base("Orc", health: 45, damage: 15, defence: 4) { }
}

public class Troll : BaseEnemy
{
    public Troll() : base("Troll", health: 80, damage: 25, defence: 10) { }
}