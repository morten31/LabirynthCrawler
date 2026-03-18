using LabirynthCrawler.Model.Items;
namespace LabirynthCrawler.Model.PlayerModel;

public class Inventory
{
    private int _gold = 0;
    private int _coins = 0;

    private ItemBase? _leftHand;
    private ItemBase? _rightHand;

    private List<ItemBase> _itemList = [];

    public int GetCoinsCount() => _coins;
    public int GetGoldCount() => _gold;

    public void ChangeGoldCount(int gold) => _gold += gold;
    public void ChangeCoinsCount(int coins) => _coins += coins; 
    
    public (ItemBase?, ItemBase?) GetHandsContent() => (_leftHand, _rightHand);
    public void AddToInventory(ItemBase item) => _itemList.Add(item);

    public int GetItemCount() => _itemList.Count;
    
    public IReadOnlyList<ItemBase> GetItems() => _itemList;
    public void EquipItem(char hand, int idx)
    {
        ItemBase item = _itemList[idx];
        if (item.IsTwoHanded())
        {
            UnEquipItem('L');
            UnEquipItem('R');
            _rightHand = item;
            _leftHand = item;
            return;
        }
        UnEquipItem(hand);
        if (hand == 'L')
        {
            if (_rightHand == item)
                UnEquipItem('R');
            _leftHand = item;
        }
        else
        {
            if (_leftHand == item)
                UnEquipItem('L');
            _rightHand = item;
        }
    }

    public void UnEquipItem(char hand)
    {
        if (hand == 'L')
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

    public IPickable RemoveFromInventory(int idx)
    {
        IPickable item = _itemList[idx];
        if (_leftHand == item)
        {
            UnEquipItem('L');
        }
        else if (_rightHand == item)
        {
            UnEquipItem('R');
        }
        
        _itemList.RemoveAt(idx);
        return item;
    }
    
}