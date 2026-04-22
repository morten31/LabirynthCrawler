
using LabirynthCrawler.Model;
using LabirynthCrawler.Model.Logger;

namespace LabirynthCrawler.Controller.InputHandling.Handlers;

public class WrongInputHandler : BaseHandler
{
    public override bool Handle(ConsoleKeyInfo key, GameModel model)
    {
        GameLogger.Instance.Log($"Naciśnięto nieznany przycisk: {key.KeyChar}");
        return base.Handle(key, model);
    }
}