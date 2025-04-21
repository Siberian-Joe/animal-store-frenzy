using NewCore.Services.UI.Handlers;
using NewCore.ViewModels;
using NewCore.Views.UI;

namespace NewCore.Services.UI.Factories
{
    public interface IPanelHandlerFactory<in TPanel, TViewModel>
        where TPanel : PanelBinder<TViewModel>
        where TViewModel : IViewModel
    {
        IPanelHandler<TViewModel> Create(
            TPanel view,
            TViewModel viewModel,
            PanelService root);
    }
}