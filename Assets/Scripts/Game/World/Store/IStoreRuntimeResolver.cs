using Game.World.EntityRuntime;

namespace Game.World.Store
{
    public interface IStoreRuntimeResolver
    {
        bool TryGetStatus(EntityId storeId, out IStoreStatus status);
        bool TryGetStatusWriter(EntityId storeId, out IStoreStatusWriter status);
        bool TryGetOpenStore(out IStoreStatus status);
        bool TryGetShift(EntityId storeId, out IStoreShift shift);
        bool TryGetShiftWriter(EntityId storeId, out IStoreShiftWriter shift);
        bool TryGetAnyShift(out IStoreShift shift);
        bool TryGetAnyShiftWriter(out IStoreShiftWriter shift);
        bool TryGetAnyShiftRewardPolicy(out IStoreShiftRewardPolicy rewardPolicy);
        bool TryGetAnyCycleProgressWriter(out ICustomerCycleProgressWriter progress);
    }
}
