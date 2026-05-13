using Game.World.EntityRuntime;

namespace Game.World.Store
{
    public interface IStoreRuntimeResolver
    {
        bool TryGetStatus(EntityId storeId, out IStoreStatus status);
        bool TryGetStatusWriter(EntityId storeId, out IStoreStatusWriter status);
        bool TryGetOpenStore(out IStoreStatus status);
        bool TryGetAnyCycleProgressWriter(out ICustomerCycleProgressWriter progress);
    }
}
