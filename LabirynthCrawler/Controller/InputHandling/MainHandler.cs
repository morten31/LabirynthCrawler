using LabirynthCrawler.Controller.InputHandling.Handlers;
using LabirynthCrawler.Model;
using LabirynthCrawler.View;

namespace LabirynthCrawler.Controller.InputHandling;

public class MainHandler
{
    private BaseHandler _start = new StartEndHandler();
    private bool _shouldRun = true;
    public void Initialize()
    {
        InitializeInputChain(_start);
    }
    
    public void RunGame(GameModel model, FrameRenderer renderer)
    {
        while (_shouldRun)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                _shouldRun = _start.Handle(key, model);
                renderer.Render(model);
            }
        }
    }

    private void InitializeInputChain(BaseHandler start)
    {
        var logHandler = new LogViewHandler();
        var moveHandler = new MoveHandler();
        var pickUpHandler = new PickUpHandler();
        var dropHandler = new DropHandler();
        var equipHandler = new EquipHandler();
        var combatHandler = new CombatHandler();
        var wrongInputHandler = new WrongInputHandler();
            
        start.SetNext(logHandler);
        logHandler.SetNext(moveHandler);
        moveHandler.SetNext(pickUpHandler);
        pickUpHandler.SetNext(dropHandler);
        dropHandler.SetNext(equipHandler);
        equipHandler.SetNext(combatHandler);
        combatHandler.SetNext(wrongInputHandler); 
    }
}