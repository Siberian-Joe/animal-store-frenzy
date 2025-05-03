using System.Linq;
using NewCore.Domain;
using R3;

namespace NewCore.Data
{
    public class GameStateProxy : Proxy<GameState>
    {
        public ProxyCollection<Shelf, ShelfProxy> Shelves { get; private set; }
        public ProxyCollection<Customer, CustomerProxy> Customers { get; private set; }

        public override void Initialize(GameState model)
        {
            Shelves = new ProxyCollection<Shelf, ShelfProxy>(model.Shelves).AddTo(Disposables);
            Customers = new ProxyCollection<Customer, CustomerProxy>(model.Customers).AddTo(Disposables);
        }

        public override GameState ToModel()
        {
            return new GameState
            {
                Shelves = Shelves.Select(proxy => proxy.ToModel()).ToList(),
                Customers = Customers.Select(proxy => proxy.ToModel()).ToList()
            };
        }
    }
}