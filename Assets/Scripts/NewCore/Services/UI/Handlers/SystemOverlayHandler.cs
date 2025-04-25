using NewCore.Services.UI.Registries;
using NewCore.ViewModels;
using NewCore.Views.UI;

namespace NewCore.Services.UI.Handlers
{
    public class SystemOverlayHandler<TPanel, TViewModel> : PanelHandler<TPanel, TViewModel>
        where TPanel : PanelBinder<TViewModel>, ISystemOverlay
        where TViewModel : IViewModel
    {
        private readonly ISystemOverlayRegistry _registry;

        public SystemOverlayHandler(TPanel panel, TViewModel context, IUIContainerRoot uiRoots,
            ISystemOverlayRegistry registry) : base(panel, context, uiRoots) => _registry = registry;

        public override void Open()
        {
            _registry.RegisterSystemOverlay(this);
            Panel.transform.SetParent(UIRoots.SystemOverlayContainer, false);
            Panel.Open();
        }
    }
}