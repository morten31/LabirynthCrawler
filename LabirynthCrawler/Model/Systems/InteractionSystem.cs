using LabirynthCrawler.Model.Board;
using LabirynthCrawler.Model.Items;
using LabirynthCrawler.Model.Logger;
using LabirynthCrawler.Model.PlayerModel;

namespace LabirynthCrawler.Model.Systems;

public static class InteractionSystem
{
    public static bool PickUpItem(GameModel model, int playerId)
    {
        var p = model.GetPlayer(playerId);
        if (p == null || p.IsDead) return false;

        Tile tile = model.GetMap().GetTile(p.GetX(), p.GetY());
        if (tile.GetTileItems.Count <= 0)
        {
            GameLogger.Instance.Log("No item to pick up!", LogLevel.Trace, playerId);
            return false;
        }

        IItem item = tile.GetTileItems.First();
        item.OnPickUp(p);
        tile.RemoveItem(tile.GetTileItems.IndexOf(item));
        p.MakeNoise(item);
        GameLogger.Instance.Log($"Picked up item: {item.Name}", LogLevel.Info, playerId);
        return true;
    }

    public static bool DropItem(GameModel model, int playerId, int inIdx)
    {
        var p = model.GetPlayer(playerId);
        if (p == null || p.IsDead) return false;

        int idx = inIdx - 1;
        Inventory inventory = p.GetInventory();
        if (idx < 0 || inventory.GetItemCount() < idx + 1)
            return false;

        IItem item = inventory.RemoveFromInventory(idx);
        model.GetMap().AddItem(p.GetX(), p.GetY(), item);
        GameLogger.Instance.Log($"Dropped item: {item.Name}", LogLevel.Info, playerId);
        return true;
    }

    public static bool EquipItem(GameModel model, int playerId, HandSlot hand, int inIdx)
    {
        var p = model.GetPlayer(playerId);
        if (p == null || p.IsDead) return false;

        int idx = inIdx - 1;
        Inventory inventory = p.GetInventory();
        if (idx < 0 || inventory.GetItemCount() < idx + 1)
            return false;

        inventory.EquipItem(hand, idx);
        GameLogger.Instance.Log("Equipped item in hand", LogLevel.Info, playerId);
        return true;
    }
}