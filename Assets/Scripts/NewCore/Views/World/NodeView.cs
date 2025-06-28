using NewCore.Factories;
using IViewModel = NewCore.ViewModels.IViewModel;

namespace NewCore.Views.World
{
    public class NodeView<TViewModel> : View<TViewModel>, INodeView where TViewModel : IViewModel
    {
        public void Initialize(IViewModelFactory factory) => Bind(factory.Create<TViewModel>());
    }
}