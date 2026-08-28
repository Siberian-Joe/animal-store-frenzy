using R3;

namespace Game.World.GameTime
{
    public interface IGameTimeReader
    {
        GameTimeSnapshot Current { get; }
        Observable<GameTimeSnapshot> MinuteChanged { get; }
    }
}
