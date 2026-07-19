using LabirynthCrawler.Model.PlayerModel;

namespace LabirynthCrawler.Model.Items.Weapons;


public class Calka : HeavyWeapon
{
    public Calka() : base("Całka", 12) { }
}

public class Pochodna : LightWeapon
{
    public Pochodna() : base("Pochodna", 5) { }
}

public class TwierdzenieBanacha : MagicWeapon
{
    public TwierdzenieBanacha() : base("Twierdzenie Banacha", 37) { }
}

public class ZabEulera : MagicWeapon
{
    public ZabEulera() : base("Ząb Eulera", 7) { }
}

public class GoldenApple : QuestItem { public GoldenApple() : base("Golden Apple") { } }
public class WoodenAxe : HeavyWeapon { public WoodenAxe() : base("Wooden Axe", 8) { } }
public class IronSword : LightWeapon { public IronSword() : base("Iron Sword", 12) { } }
public class Bow : LightWeapon { public Bow() : base("Bow", 10) { } }
public class VoidSword : MagicWeapon { public VoidSword() : base("Void Sword", 25) { } }
public class ObsidianMace : HeavyWeapon { public ObsidianMace() : base("Obsidian Mace", 30) { } }
public class ShulkerShield : LightWeapon { public ShulkerShield() : base("Shulker Shield", 5) { } }
public class GoldenSword : LightWeapon { public GoldenSword() : base("Golden Sword", 15) { } }
public class NetheriteAxe : HeavyWeapon { public NetheriteAxe() : base("Netherite Axe", 22) { } }
public class BlazeRod : MagicWeapon { public BlazeRod() : base("Blaze Rod", 18) { } }

// Artifacts
public class DiamondSword : ArtifactWeapon { public DiamondSword() : base("Diamond Sword", 25) { } }
public class DragonSlayerHalberd : ArtifactWeapon 
{ 
    public DragonSlayerHalberd() : base("Dragon Slayer Halberd", 50) { } 

    public override Attributes GetAttributes() => new(power: 20, wisdom: 25, luck: 5);
}
public class WitherScythe : ArtifactWeapon 
{ 
    public WitherScythe() : base("Wither Scythe", 45) { } 

    public override Attributes GetAttributes() => new (power: 15, aggression: 20, health: -20, luck: -10);
}