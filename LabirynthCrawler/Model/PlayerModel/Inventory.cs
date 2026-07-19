using LabirynthCrawler.Model.Items;
using LabirynthCrawler.Model.Logger;

namespace LabirynthCrawler.Model.PlayerModel;

public enum HandSlot
{
    Left,
    Right
}

public class Inventory
{
    private int _gold = 0;
    private int _coins = 0;

    private IItem? _leftHand;
    private IItem? _rightHand;
    private List<IItem> _itemList = [];

    public int GetCoinsCount() => _coins;
    public int GetGoldCount() => _gold;

    public void ChangeGoldCount(int gold) => _gold += gold;
    public void ChangeCoinsCount(int coins) => _coins += coins; 
    
    public (IItem?, IItem?) GetHandsContent() => (_leftHand, _rightHand);
    public void AddToInventory(IItem item) => _itemList.Add(item);

    public int GetItemCount() => _itemList.Count;
    
    public IReadOnlyList<IItem> GetItems() => _itemList;
    public bool EquipItem(HandSlot hand, int idx)
    {
        IItem item = _itemList[idx];
        if (item.IsTwoHanded())
        {
            UnEquipItem(HandSlot.Left);
            UnEquipItem(HandSlot.Right);
            _rightHand = item;
            _leftHand = item;
            return true;
        }
        UnEquipItem(hand);
        if (hand == HandSlot.Left)
        {
            if (_rightHand == item)
                UnEquipItem(HandSlot.Right);
            _leftHand = item;
        }
        else
        {
            if (_leftHand == item)
                UnEquipItem(HandSlot.Left);
            _rightHand = item;
        }

        return true;
    }

    public void UnEquipItem(HandSlot hand)
    {
        if (hand == HandSlot.Left)
        {
            if (_leftHand != null && _leftHand.IsTwoHanded())
            {
                _rightHand = null;
                _leftHand = null;
                return;
            }
            _leftHand = null;
        }
        else
        {
            if (_rightHand != null && _rightHand.IsTwoHanded()) 
            {
                _rightHand = null;
                _leftHand = null;
                return;
            }
            _rightHand = null;
        }
    }

    public IItem RemoveFromInventory(int idx)
    {
        IItem item = _itemList[idx];
        if (_leftHand == item)
        {
            UnEquipItem(HandSlot.Left);
        }
        else if (_rightHand == item)
        {
            UnEquipItem(HandSlot.Right);
        }
        
        _itemList.RemoveAt(idx);
        return item;
    }
    
    public List<IItem> GetEquippedItems()
    {
        List<IItem> itemsInHands = new List<IItem>();
        (IItem? item1, IItem? item2) = GetHandsContent();

        if (item1 != null && item1.IsTwoHanded())
        {
            itemsInHands.Add(item1);
        }
        else
        {
            if (item1 != null) itemsInHands.Add(item1);
            if (item2 != null) itemsInHands.Add(item2);
        }

        return itemsInHands;
    }

    public Attributes GetEquippedAttributes()
    {
        Attributes total = new Attributes();
        foreach (var item in GetEquippedItems())
        {
            total = total.Add(item.GetAttributes());
        }
        return total;
    }

}