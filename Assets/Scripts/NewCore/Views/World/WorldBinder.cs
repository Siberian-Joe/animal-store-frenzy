using System.Collections.Generic;
using NewCore.ViewModels;
using ObservableCollections;
using R3;
using UnityEngine;

namespace NewCore.Views.World
{
    public class WorldBinder : Binder<WorldViewModel>
    {
        [SerializeField] private CustomerBinder _customerBinderPrefab;

        private readonly Dictionary<string, CustomerBinder> _customers = new();

        protected override void OnBind()
        {
            foreach (var customer in ViewModel.Customers)
                CreateCustomer(customer);

            ViewModel.Customers
                .ObserveAdd()
                .Subscribe(customer => CreateCustomer(customer.Value))
                .AddTo(Disposables);

            ViewModel.Customers
                .ObserveRemove()
                .Subscribe(customer => RemoveCustomer(customer.Value))
                .AddTo(Disposables);
        }

        private void CreateCustomer(CustomerViewModel viewModel)
        {
            var customer = Instantiate(_customerBinderPrefab, transform);

            customer.Bind(viewModel);
            _customers.Add(viewModel.Id, customer);
        }

        private void RemoveCustomer(CustomerViewModel viewModel)
        {
            if (_customers.TryGetValue(viewModel.Id, out var customer))
            {
                Destroy(customer.gameObject); // TODO: Need to use pooling
                _customers.Remove(viewModel.Id);
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            foreach (var customer in _customers.Values)
                Destroy(customer.gameObject);
        }
    }
}