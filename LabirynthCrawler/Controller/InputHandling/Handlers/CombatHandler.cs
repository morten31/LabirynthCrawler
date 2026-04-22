using LabirynthCrawler.Model;

namespace LabirynthCrawler.Controller.InputHandling.Handlers;


public class CombatHandler : BaseHandler
{
    public override bool Handle(ConsoleKeyInfo key, GameModel model)
    {
        if (model.CurrentState == GameModel.GameState.GameOver)
            return base.Handle(key, model);

        if (KeyBindings.Matches(key, GameAction.AttackNormal))
        {
            model.PerformAttack(1);
            return true;
        }
        if (KeyBindings.Matches(key, GameAction.AttackStealth))
        {
            model.PerformAttack(2);
            return true;
        }
        if (KeyBindings.Matches(key, GameAction.AttackMagic))
        {
            model.PerformAttack(3);
            return true;
        }

        return base.Handle(key, model);
    }
}