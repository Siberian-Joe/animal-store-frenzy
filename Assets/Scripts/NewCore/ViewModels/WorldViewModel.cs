using NewCore.Lifecycle;
using ObservableCollections;

namespace NewCore.ViewModels
{
    public class WorldViewModel : ViewModel
    {
        public readonly IObservableCollection<CustomerViewModel> Customers; // TODO: Move to proxy

        public WorldViewModel(ICustomerLifecycle customerLifecycle) => Customers = customerLifecycle.Entities;
    }
}