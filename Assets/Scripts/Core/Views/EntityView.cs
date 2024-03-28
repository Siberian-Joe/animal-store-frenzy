using Interfaces.Core;

namespace Core.Views
{
    public abstract class EntityView<TViewModel> : View<TViewModel> where TViewModel : IEntityViewModel
    {
        protected override void Initialize()
        {
            ViewModel.SetTransform(transform);

            base.Initialize();
        }
    }
}