using System.Diagnostics;
using LabirynthCrawler.Controller.InputHandling.Handlers;
using LabirynthCrawler.Model;
using LabirynthCrawler.Model.Network;
using LabirynthCrawler.View;
using LabirynthCrawler.View.Renderers;

namespace LabirynthCrawler.Controller.InputHandling;

public class MainHandler
{
    private BaseHandler _start = new StartEndHandler();
    private bool _shouldRun = true;
    public void Initialize()
    {
        InitializeInputChain(_start);
    }

    public BaseHandler GetStartHandler() => _start;
    
    public void RunGame(GameModel model, LocalRenderer renderer, int localPlayerId, Action? onStateChanged = null)
    {
        Stopwatch tickTimer = Stopwatch.StartNew();
        int tickIntervalMs = 1000;
        
        while (_shouldRun)
        {
            bool stateChanged = false;

            if (tickTimer.ElapsedMilliseconds >= tickIntervalMs)
            {
                model.Tick();
                tickTimer.Restart();
                stateChanged = true;
            }
            
            if (Console.KeyAvailable)
            {
                PlayerActionDto? action = InputParser.ParseInput(localPlayerId, model.CurrentState.ToString());
                if (action != null)
                {
                    _shouldRun = _start.Handle(action, model);
                    stateChanged = true;
                }
            }
            
            if (stateChanged)
            {
                renderer.Render(model, localPlayerId);
                onStateChanged?.Invoke(); 
            }
            
            Thread.Sleep(1); 
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