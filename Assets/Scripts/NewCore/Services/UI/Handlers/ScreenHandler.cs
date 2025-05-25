using NewCore.Services.UI.Registries;
using NewCore.ViewModels;
using NewCore.Views.UI;
using UnityEngine;

namespace NewCore.Services.UI.Handlers
{
    public class ScreenHandler<TPanel, TViewModel> : RegistryPanelHandler<TPanel, TViewModel>
        where TPanel : PanelView<TViewModel>, IScreen
        where TViewModel : IViewModel
    {
        protected override Transform Container => UIRoots.ScreensContainer;

        public ScreenHandler(TPanel panel, TViewModel context, IUIContainerRoot uiRoots, IScreenRegistry registry) :
            base(panel, context, uiRoots, registry)
        {
        }
    }
}