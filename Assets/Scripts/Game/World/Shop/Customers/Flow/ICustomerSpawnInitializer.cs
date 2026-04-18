namespace Game.World.Shop.Customers.Flow
{
    public interface ICustomerSpawnInitializer
    {
        void ApplySpawnRequest(CustomerSpawnRequest request);
    }
}