namespace Game.World.Store
{
    public interface IStoreShift
    {
        StoreShiftStatus Status { get; }
        int ServedCustomers { get; }
        int RequiredCustomers { get; }
        int ActiveCustomers { get; }
        int Revenue { get; }
        bool CanSpawnCustomer { get; }
        bool CanCloseStore { get; }
    }
}