using NewCore.Services.UI.Registries;
using NewCore.ViewModels;
using NewCore.Views.UI;

namespace NewCore.Services.UI.Handlers
{
    public class OverlayHandler<TPanel, TViewModel> : PanelHandler<TPanel, TViewModel>
        where TPanel : PanelBinder<TViewModel>, IOverlay
        where TViewModel : IViewModel
    {
        private readonly IOverlayRegistry _registry;

        public OverlayHandler(TPanel panel, TViewModel context, IUIContainerRoot uiRoots, IOverlayRegistry registry) :
            base(panel, context, uiRoots) => _registry = registry;

        public override void Open()
        {
            _registry.RegisterOverlay(this);
            Panel.transform.SetParent(UIRoots.OverlayContainer, false);
            Panel.transform.SetSiblingIndex(Panel.Order);
            Panel.Open();
        }

        public override void Close()
        {
            _registry.UnregisterOverlay(this);
            base.Close();
        }
    }
}