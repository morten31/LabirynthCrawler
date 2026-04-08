using LabirynthCrawler.Model.Items;

namespace LabirynthCrawler.Model.PlayerModel;

public class Player((int, int) startPosition)
{
    private Attributes _attributes = new Attributes(10, 10, 100, 10, 10, 10);
    private int _x = startPosition.Item1;
    private int _y = startPosition.Item2;
    private Inventory _inventory = new Inventory();

    public int GetX() => _x;
    public int GetY() => _y;
    public void MoveTo(int x, int y) => (_x, _y) = (x, y);
    public Inventory GetInventory() => _inventory;
    public void AddItem(IItem item) => _inventory.AddToInventory(item);
    public Attributes GetAttributes() => _attributes;
    public Attributes GetTotalAttributes() => _attributes.Add(_inventory.GetEquippedAttributes());
}