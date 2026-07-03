using Game.World.EntityRuntime;

namespace Game.World.Store
{
    public readonly struct StoreShiftRewardContext
    {
        public StoreShiftRewardContext(EntityId customerId, int servedCustomerNumber)
        {
            CustomerId = customerId;
            ServedCustomerNumber = servedCustomerNumber;
        }

        public EntityId CustomerId { get; }
        public int ServedCustomerNumber { get; }
    }
}