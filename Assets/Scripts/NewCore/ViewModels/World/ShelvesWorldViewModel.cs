using NewCore.Services.Lifecycle;
using ObservableCollections;

namespace NewCore.ViewModels.World
{
    public class ShelvesWorldViewModel : ViewModel
    {
        public readonly IObservableCollection<ShelfViewModel> Shelves;

        public ShelvesWorldViewModel(IShelvesLifecycle shelvesLifecycle) => Shelves = shelvesLifecycle.Entities;
    }
}