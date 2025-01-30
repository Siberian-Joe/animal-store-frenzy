using System.Collections.Generic;
using NewCore.Data;
using NewCore.ViewModels;
using ObservableCollections;

namespace NewCore.Domain
{
    public class CustomerRegistry
    {
        private readonly ObservableList<CustomerViewModel> _customers = new();
        private readonly Dictionary<string, CustomerViewModel> _customerMap = new();

        public IObservableCollection<CustomerViewModel> Customers => _customers;

        public void Add(CustomerProxy proxy)
        {
            if (_customerMap.ContainsKey(proxy.Id))
                return;

            var viewModel = new CustomerViewModel(proxy);
            _customers.Add(viewModel);
            _customerMap[proxy.Id] = viewModel;
        }

        public void Remove(CustomerProxy proxy)
        {
            if (!_customerMap.TryGetValue(proxy.Id, out var viewModel))
                return;

            _customers.Remove(viewModel);
            _customerMap.Remove(proxy.Id);
        }
    }
}