using NewCore.Domain;

namespace NewCore.Data
{
    public class GameStateProxy : IProxy
    {
        public ProxyCollection<Shelf, ShelfProxy> Shelves { get; }
        public ProxyCollection<Domain.Customer, CustomerProxy> Customers { get; }

        public GameStateProxy(GameState gameState)
        {
            Shelves = new(gameState.Shelves);
            Customers = new(gameState.Customers);
        }

        public void Dispose()
        {
            Shelves?.Dispose();
            Customers?.Dispose();
        }
    }
}