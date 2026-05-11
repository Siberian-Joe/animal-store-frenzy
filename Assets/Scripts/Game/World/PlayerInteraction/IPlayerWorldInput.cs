using R3;

namespace Game.World.PlayerInteraction
{
    public interface IPlayerWorldInput
    {
        Observable<PlayerWorldClick> Clicked { get; }
    }
}