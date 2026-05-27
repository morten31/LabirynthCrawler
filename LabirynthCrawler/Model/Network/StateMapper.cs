using LabirynthCrawler.Model.Board;
using LabirynthCrawler.Model.Logger;
using LabirynthCrawler.Model.PlayerModel;

namespace LabirynthCrawler.Model.Network;

public static class StateMapper
{
    public static GameStateDto ToDto(GameModel model, int targetPlayerId)
    {
        var p = model.GetPlayer(targetPlayerId);
        string state = model.CurrentState == GameModel.GameState.GameOver ? "GameOver" : 
            (p != null && p.IsViewingLog ? "ViewingLog" : "Playing");
        
        var dto = new GameStateDto
        {
            MapWidth = Map.Width,
            MapHeight = Map.Height,
            CurrentState = state
        };

        lock (model.StateLock)
        {
            Map map = model.GetMap();
            for (int y = 0; y < Map.Height; y++)
            {
                for (int x = 0; x < Map.Width; x++)
                {
                    Tile tile = map.GetTile(x, y);
                    string symbol = tile.GetSymbol();
                    
                    if (!tile.IsWall() && symbol != " ")
                    {
                        dto.Tiles.Add(new TileDto
                        {
                            X = x,
                            Y = y,
                            Symbol = symbol,
                            IsWall = tile.IsWall()
                        });
                    }
                }
            }

            foreach (var kvp in model.Players)
            {
                int id = kvp.Key;
                Player pp = kvp.Value;
                Attributes attr = pp.GetTotalAttributes();
                Inventory inv = pp.GetInventory();
                var hands = inv.GetHandsContent();

                dto.Players[id] = new PlayerDto
                {
                    Id = id,
                    X = pp.GetX(),
                    Y = pp.GetY(),
                    IsDead = pp.IsDead,
                    Health = attr.Health,
                    Power = attr.Power,
                    Agility = attr.Agility,
                    Luck = attr.Luck,
                    Aggression = attr.Aggression,
                    Wisdom = attr.Wisdom,
                    Coins = inv.GetCoinsCount(),
                    Gold = inv.GetGoldCount(),
                    Inventory = inv.GetItems().Select(i => i.ToString()).ToList(),
                    LeftHand = hands.Item1?.ToString() ?? "Empty",
                    RightHand = hands.Item2?.ToString() ?? "Empty"
                };
            }

            dto.RecentLogs = GameLogger.Instance.GetRecentLogs(5, targetPlayerId).ToList();
            dto.ActionMessages = model.GetInstructionBuilder().GetCompleteMessages();
        }

        return dto;
    }
}