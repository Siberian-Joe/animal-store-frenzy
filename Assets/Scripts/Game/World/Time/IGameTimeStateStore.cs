namespace Game.World.GameTime
{
    public interface IGameTimeStateStore
    {
        bool TryLoad(out GameTimeState state);
        void Save(GameTimeState state);
    }
}
