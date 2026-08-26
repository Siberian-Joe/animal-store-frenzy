namespace Game.World.Store
{
    public interface IStoreRuntimeRegistry
    {
        void Register(StoreStatusPart status);
        void Unregister(StoreStatusPart status);

        void Register(StoreCustomerCycleProgressPart progress);
        void Unregister(StoreCustomerCycleProgressPart progress);
    }
}
