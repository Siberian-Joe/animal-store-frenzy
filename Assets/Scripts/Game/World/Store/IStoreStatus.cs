namespace Game.World.Store
{
    public interface IStoreStatus
    {
        StoreStatus Status { get; }
        bool IsOpen { get; }
    }
}
