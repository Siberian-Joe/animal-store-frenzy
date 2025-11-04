using NewCore.Services.Lifecycle;
using ObservableCollections;

namespace NewCore.ViewModels.World
{
    public class CustomersWorldViewModel : ViewModel
    {
        // TODO: Move to proxy
        public readonly IObservableCollection<CustomerViewModel> Customers;

        public CustomersWorldViewModel(ICustomerLifecycle customerLifecycle) => Customers = customerLifecycle.Entities;
    }
}