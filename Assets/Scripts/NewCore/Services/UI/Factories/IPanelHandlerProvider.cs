using NewCore.Services.UI.Handlers;
using NewCore.ViewModels;
using NewCore.Views.UI;

namespace NewCore.Services.UI.Factories
{
    public interface IPanelHandlerProvider
    {
        IPanelHandler Provide(IPanel panel, IViewModel viewModel, IUIContainerRoot uiRoots);
    }
}