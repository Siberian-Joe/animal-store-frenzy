using R3;

namespace Game.World.Store
{
    public interface IStoreStatus
    {
        StoreStatus Status { get; }
        bool IsOpen { get; }
        Observable<Unit> Changed { get; }
    }
}
