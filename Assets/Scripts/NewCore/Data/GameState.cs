using System.Collections.Generic;
using NewCore.Domain;
using NewCore.Extensions;
using NewCore.Modules.Interaction;
using ObservableCollections;
using R3;

namespace NewCore.Data
{
    public class GameState : Proxy<GameStateData>
    {
        public ReactiveProperty<Player> Player { get; }
        public ObservableList<Shelf> Shelves { get; }
        public ObservableList<Customer> Customers { get; }

        public GameState(GameStateData model, IProxyFactory proxyFactory) : base(model)
        {
            Player = new ReactiveProperty<Player>(model.Player?.ToProxy<Player>(proxyFactory));
            Shelves = new ObservableList<Shelf>();
            Customers = new ObservableList<Customer>();

            Shelves
                .InitializeFromModels(model.Shelves, proxyFactory)
                .AddTo(Disposables);

            Customers
                .InitializeFromModels(model.Customers, proxyFactory)
                .AddTo(Disposables);
        }

        protected override GameStateData CreateModel()
        {
            return new GameStateData
            {
                Player = Player?.Value?.ToModel(),
                Shelves = new List<ShelfData>(Shelves.ToModels()),
                Customers = new List<CustomerData>(Customers.ToModels())
            };
        }
    }
}