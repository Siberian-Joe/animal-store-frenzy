namespace Game.World.Shop.Customers.Flow
{
    public interface ICustomerSpawnPointLocator
    {
        bool HasSpawnPoints { get; }

        bool TryGetRandomSpawnPoint(out CustomerSpawnPointPart spawnPoint);
    }
}