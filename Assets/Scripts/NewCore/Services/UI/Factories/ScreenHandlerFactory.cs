using NewCore.Services.UI.Handlers;
using NewCore.ViewModels;
using NewCore.Views.UI;

namespace NewCore.Services.UI.Factories
{
    public class ScreenHandlerFactory<TPanel, TViewModel>
        : IPanelHandlerFactory<TPanel, TViewModel>
        where TPanel : PanelBinder<TViewModel>, IScreen
        where TViewModel : IViewModel
    {
        public IPanelHandler<TViewModel> Create(
            TPanel view,
            TViewModel vm,
            PanelService root)
            => new ScreenHandler<TPanel, TViewModel>(view, vm, root);
    }
}