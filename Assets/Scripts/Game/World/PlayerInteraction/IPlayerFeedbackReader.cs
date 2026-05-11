using R3;

namespace Game.World.PlayerInteraction
{
    public interface IPlayerFeedbackReader
    {
        string CurrentMessage { get; }
        Observable<string> MessageShown { get; }
    }
}