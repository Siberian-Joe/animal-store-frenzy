using Cysharp.Threading.Tasks;
using NewCore.Commands;
using NewCore.Data;
using NewCore.Domain;
using NewCore.Factories;
using NewCore.ViewModels;
using UnityEngine;

namespace NewCore.Services.Lifecycle
{
    public class CustomerLifecycle : EntityLifecycle<Customer, CustomerProxy, CustomerViewModel>, ICustomerLifecycle
    {
        private readonly ICommandProcessor _commandProcessor;

        public CustomerLifecycle(IViewModelFactory viewModelFactory, ICommandProcessor commandProcessor) :
            base(viewModelFactory) => _commandProcessor = commandProcessor;

        public UniTask<bool> TrySpawnCustomer(string customerType, Vector3Int position) =>
            _commandProcessor.TryProcessAsync(new SpawnCustomerCommand(customerType, position));
    }
}