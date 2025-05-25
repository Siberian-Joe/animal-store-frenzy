using NewCore.Domain;
using NewCore.Extensions;
using ObservableCollections;

namespace NewCore.Data
{
    public class GameState : Proxy<GameStateData>
    {
        public ObservableList<Shelf> Shelves { get; private set; }
        public ObservableList<Customer> Customers { get; private set; }

        public override void Initialize(GameStateData model)
        {
            Shelves = new ObservableList<Shelf>();
            Customers = new ObservableList<Customer>();

            Shelves.InitializeFromModels(model.Shelves, Disposables);
            Customers.InitializeFromModels(model.Customers, Disposables);
        }

        public override GameStateData ToModel()
        {
            return new GameStateData
            {
                Shelves = Shelves.ToModelList<ShelfData, Shelf>(),
                Customers = Customers.ToModelList<CustomerData, Customer>()
            };
        }
    }
}