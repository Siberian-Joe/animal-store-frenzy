using System.Collections.Generic;
using NewCore.ViewModels;
using ObservableCollections;
using R3;
using UnityEngine;

namespace NewCore.Views.World
{
    public class WorldView : View<WorldViewModel>
    {
        [SerializeField] private CustomerView _customerViewPrefab;

        private readonly Dictionary<string, CustomerView> _customers = new();

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
            var customer = Instantiate(_customerViewPrefab, transform);

            customer.Bind(viewModel);
            _customers.Add(viewModel.Id, customer);
        }

        private void RemoveCustomer(CustomerViewModel viewModel)
        {
            if (_customers.TryGetValue(viewModel.Id, out var customer))
            {
                // TODO: Need to use pooling
                Destroy(customer.gameObject);
                _customers.Remove(viewModel.Id);
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            foreach (var customer in _customers.Values)
            {
                Destroy(customer.gameObject);
            }

            _customers.Clear();
        }
    }
}