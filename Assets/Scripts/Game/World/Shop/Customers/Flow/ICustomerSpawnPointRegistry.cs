namespace Game.World.Shop.Customers.Flow
{
    public interface ICustomerSpawnPointRegistry
    {
        void Register(CustomerSpawnPointPart spawnPoint);
        void Unregister(CustomerSpawnPointPart spawnPoint);
    }
}