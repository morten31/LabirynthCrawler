using LabirynthCrawler.Model;

namespace LabirynthCrawler.Controller.InputHandling.Handlers;

public class DropHandler : BaseHandler
{
    public override bool Handle(ConsoleKeyInfo key, GameModel model)
    {
        if (KeyBindings.Matches(key, GameAction.Drop))
        {
            ConsoleKeyInfo key2 = Console.ReadKey(true);

            if (char.IsDigit(key2.KeyChar))
            {
                int index = int.Parse(key2.KeyChar.ToString());
                
                if(model.DropItem(index))
                    return true; 
            }
        }
        return base.Handle(key, model);
    }
}