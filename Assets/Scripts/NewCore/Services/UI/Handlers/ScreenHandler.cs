using NewCore.Services.UI.Registries;
using NewCore.ViewModels;
using NewCore.Views.UI;

namespace NewCore.Services.UI.Handlers
{
    public class ScreenHandler<TPanel, TViewModel> : PanelHandler<TPanel, TViewModel>
        where TPanel : PanelBinder<TViewModel>, IScreen
        where TViewModel : IViewModel
    {
        private readonly IScreenRegistry _registry;

        public ScreenHandler(TPanel panel, TViewModel context, IUIContainerRoot uiRoots, IScreenRegistry registry) :
            base(panel, context, uiRoots) => _registry = registry;

        public override void Open()
        {
            _registry.RegisterScreen(this);
            Panel.transform.SetParent(UIRoots.ScreensContainer, false);
            Panel.Open();
        }
    }
}