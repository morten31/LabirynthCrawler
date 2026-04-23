using LabirynthCrawler.Model.PlayerModel;

namespace LabirynthCrawler.Model.Items.Weapons;

public abstract class ArtifactWeapon : HeavyWeapon
{
    public ArtifactWeapon(string name, int damage) : base($"[ARTIFACT] {name}", damage) { }
    public override char GetSymbol() => 'A';
    public override Attributes GetAttributes() => new(power: 5, health: 50, luck: 10);
}