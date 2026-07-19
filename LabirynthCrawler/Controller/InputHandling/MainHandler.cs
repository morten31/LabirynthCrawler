using System.Diagnostics;
using LabirynthCrawler.Controller.InputHandling.Handlers;
using LabirynthCrawler.Model;
using LabirynthCrawler.Model.Network;
using LabirynthCrawler.Model.Systems;
using LabirynthCrawler.View;
using LabirynthCrawler.View.Renderers;

namespace LabirynthCrawler.Controller.InputHandling;

public class MainHandler
{
    private BaseHandler _start = new StartEndHandler();
    private bool _shouldRun = true;
    private EnemyAISystem _enemyAi = new EnemyAISystem();
    
    private LocalInputManager _inputManager = new LocalInputManager();

    public void Initialize()
    {
        InitializeInputChain(_start);
    }

    public BaseHandler GetStartHandler() => _start;
    
    public void RunGame(GameModel model, LocalRenderer renderer, int localPlayerId, Action? onStateChanged = null)
    {
        Stopwatch tickTimer = Stopwatch.StartNew();
        int tickIntervalMs = 1000;
        bool isViewingLog = false;
        
        while (_shouldRun)
        {
            bool stateChanged = false;

            if (tickTimer.ElapsedMilliseconds >= tickIntervalMs)
            {
                _enemyAi.Tick(model);
                tickTimer.Restart();
                stateChanged = true;
            }
            
            if (Console.KeyAvailable)
            {
                var p = model.GetPlayer(localPlayerId);
                string stateStr = model.CurrentState == GameModel.GameState.GameOver ? "GameOver" :
                    (isViewingLog ? "ViewingLog" : "Playing");
                
                var keyInfo = Console.ReadKey(true);
                PlayerActionDto? action = _inputManager.ProcessInput(keyInfo, localPlayerId, stateStr);
                
                if (action != null)
                {
                    if (action.ActionType == "ToggleLog" || action.ActionType == "Escape")
                    {
                        isViewingLog = !isViewingLog;
                        stateChanged = true;
                    }
                    else
                    {
                        lock (model.StateLock)
                        {
                            _shouldRun = _start.Handle(action, model);
                        }
                        stateChanged = true;
                    }
                }
            }
            
            if (stateChanged)
            {
                renderer.Render(model, localPlayerId, isViewingLog);
                onStateChanged?.Invoke(); 
            }
            
            Thread.Sleep(5); 
        }
    }

    private void InitializeInputChain(BaseHandler start)
    {
        var moveHandler = new MoveHandler();
        var pickUpHandler = new PickUpHandler();
        var dropHandler = new DropHandler();
        var equipHandler = new EquipHandler();
        var combatHandler = new CombatHandler();
        var wrongInputHandler = new WrongInputHandler();
            
        start.SetNext(moveHandler);
        moveHandler.SetNext(pickUpHandler);
        pickUpHandler.SetNext(dropHandler);
        dropHandler.SetNext(equipHandler);
        equipHandler.SetNext(combatHandler);
        combatHandler.SetNext(wrongInputHandler); 
    }
}