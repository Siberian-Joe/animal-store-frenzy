using NewCore.Domain;
using NewCore.Extensions;
using ObservableCollections;
using R3;

namespace NewCore.Data
{
    public class GameState : Proxy<GameStateData>
    {
        public ReactiveProperty<Player> Player { get; private set; }
        public ObservableList<Shelf> Shelves { get; private set; }
        public ObservableList<Customer> Customers { get; private set; }

        public override void Initialize(GameStateData data)
        {
            base.Initialize(data);

            Player = new ReactiveProperty<Player>(data.Player?.ToProxy<PlayerData, Player>());
            Shelves = new ObservableList<Shelf>();
            Customers = new ObservableList<Customer>();

            Shelves.InitializeFromModels(data.Shelves, Disposables);
            Customers.InitializeFromModels(data.Customers, Disposables);
        }

        public override GameStateData ToModel()
        {
            return new GameStateData
            {
                Player = Player.Value.ToModel(),
                Shelves = Shelves.ToModelList<ShelfData, Shelf>(),
                Customers = Customers.ToModelList<CustomerData, Customer>()
            };
        }
    }
}