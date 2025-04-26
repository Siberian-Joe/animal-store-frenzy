using NewCore.Services.UI.Handlers;
using NewCore.ViewModels;
using NewCore.Views.UI;

namespace NewCore.Services.UI.Factories
{
    public interface IPanelHandlerFactory
    {
        IPanelHandler<TViewModel> Create<TPanel, TViewModel>(TPanel panel, TViewModel viewModel,
            IUIContainerRoot uiRoots) where TPanel : PanelBinder<TViewModel> where TViewModel : class, IViewModel;
    }
}