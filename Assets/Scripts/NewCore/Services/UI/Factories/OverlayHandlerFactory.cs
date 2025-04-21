using NewCore.Services.UI.Handlers;
using NewCore.ViewModels;
using NewCore.Views.UI;

namespace NewCore.Services.UI.Factories
{
    public class OverlayHandlerFactory<TPanel, TViewModel>
        : IPanelHandlerFactory<TPanel, TViewModel>
        where TPanel : PanelBinder<TViewModel>, IOverlay
        where TViewModel : IViewModel
    {
        public IPanelHandler<TViewModel> Create(
            TPanel view,
            TViewModel vm,
            PanelService root)
            => new OverlayHandler<TPanel, TViewModel>(view, vm, root);
    }
}