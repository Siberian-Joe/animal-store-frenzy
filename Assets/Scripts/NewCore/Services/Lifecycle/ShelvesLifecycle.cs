using NewCore.Data;
using NewCore.Factories;
using NewCore.ViewModels.World;

namespace NewCore.Services.Lifecycle
{
    public class ShelvesLifecycle : EntityLifecycle<Shelf, ShelfViewModel>, IShelvesLifecycle
    {
        public ShelvesLifecycle(IViewModelFactory viewModelFactory) : base(viewModelFactory)
        {
        }
    }
}