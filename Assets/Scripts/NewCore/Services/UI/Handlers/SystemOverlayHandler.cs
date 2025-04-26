using NewCore.Services.UI.Registries;
using NewCore.ViewModels;
using NewCore.Views.UI;
using UnityEngine;

namespace NewCore.Services.UI.Handlers
{
    public class SystemOverlayHandler<TPanel, TViewModel> : RegistryPanelHandler<TPanel, TViewModel>
        where TPanel : PanelBinder<TViewModel>, ISystemOverlay
        where TViewModel : IViewModel
    {
        protected override Transform Container => UIRoots.SystemOverlayContainer;

        public SystemOverlayHandler(TPanel panel, TViewModel context, IUIContainerRoot uiRoots,
            ISystemOverlayRegistry registry) : base(panel, context, uiRoots, registry)
        {
        }
    }
}