using System;
using Cysharp.Threading.Tasks;
using NewCore.Commands;
using NewCore.Data;
using NewCore.Domain;
using NewCore.ViewModels;
using ObservableCollections;
using R3;
using UnityEngine;

namespace NewCore.Lifecycle
{
    public class CustomerLifecycle : ICustomerLifecycle, IDisposable
    {
        public IObservableCollection<CustomerViewModel> Customers => _customerRegistry.Customers;

        private readonly ICommandProcessor _commandProcessor;
        private readonly CustomerRegistry _customerRegistry = new();
        private readonly CompositeDisposable _disposables = new();

        public CustomerLifecycle(ICommandProcessor commandProcessor) => _commandProcessor = commandProcessor;

        public void Initialize(GameStateProxy gameStateProxy)
        {
            foreach (var customer in gameStateProxy.Customers)
            {
                _customerRegistry.Add(customer);
            }

            gameStateProxy.Customers
                .ObserveAdd()
                .Subscribe(added => _customerRegistry.Add(added.Value))
                .AddTo(_disposables);

            gameStateProxy.Customers
                .ObserveRemove()
                .Subscribe(removed => _customerRegistry.Remove(removed.Value))
                .AddTo(_disposables);
        }

        public UniTask<bool> TrySpawnCustomer(string customerType, Vector3Int position) =>
            _commandProcessor.TryProcessAsync(new SpawnCustomerCommand(customerType, position));

        public void Dispose() => _disposables.Dispose();
    }
}