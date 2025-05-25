using NewCore.Services.UI.Registries;
using NewCore.ViewModels;
using NewCore.Views.UI;
using UnityEngine;

namespace NewCore.Services.UI.Handlers
{
    public class OverlayHandler<TPanel, TViewModel> : RegistryPanelHandler<TPanel, TViewModel>
        where TPanel : PanelView<TViewModel>, IOverlay
        where TViewModel : IViewModel
    {
        protected override Transform Container => UIRoots.OverlayContainer;

        public OverlayHandler(TPanel panel, TViewModel context, IUIContainerRoot uiRoots, IOverlayRegistry registry) :
            base(panel, context, uiRoots, registry)
        {
        }
    }
}