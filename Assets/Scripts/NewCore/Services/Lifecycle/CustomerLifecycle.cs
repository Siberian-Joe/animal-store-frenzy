using NewCore.Commands;
using NewCore.Data;
using NewCore.Factories;
using NewCore.ViewModels.World;
using UnityEngine;

namespace NewCore.Services.Lifecycle
{
    public class CustomerLifecycle : EntityLifecycle<Customer, CustomerViewModel>, ICustomerLifecycle
    {
        private readonly ICommandProcessor _commandProcessor;

        public CustomerLifecycle(IViewModelFactory viewModelFactory, ICommandProcessor commandProcessor) :
            base(viewModelFactory) => _commandProcessor = commandProcessor;

        public bool TrySpawnCustomer(string customerType, Vector2 position) =>
            _commandProcessor.TryProcess(new SpawnCustomerCommand(customerType, position));
    }
}