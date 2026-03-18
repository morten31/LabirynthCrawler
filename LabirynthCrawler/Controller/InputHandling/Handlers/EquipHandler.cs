using LabirynthCrawler.Model;

namespace LabirynthCrawler.Controller.InputHandling.Handlers;

public class EquipHandler : BaseHandler
{
    public override bool Handle(ConsoleKeyInfo key, GameModel model)
    {
        if (key.Key == ConsoleKey.K || key.Key == ConsoleKey.L) // left or right hand
        {
            char hand = (key.Key == ConsoleKey.K) ? 'L' : 'R';
            ConsoleKeyInfo key2 = Console.ReadKey(true);

            if (char.IsDigit(key2.KeyChar))
            {
                int index = int.Parse(key2.KeyChar.ToString());
                
                if (model.EquipItem(hand, index))
                    return true; 
            }
        }
        return base.Handle(key, model);
    }
}