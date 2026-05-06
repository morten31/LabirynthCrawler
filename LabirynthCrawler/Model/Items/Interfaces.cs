using LabirynthCrawler.Model.Combat.Visitors;
using LabirynthCrawler.Model.PlayerModel;

namespace LabirynthCrawler.Model.Items;

public interface IItem
{
    string Name { get; } 
    char GetSymbol();
    bool IsTwoHanded();
    void OnPickUp(Player player);
    Attributes GetAttributes(); 
    public string ToString();
    (int Damage, int Defense) Accept(IAttackVisitor visitor);
    int GetSoundRange();
}
