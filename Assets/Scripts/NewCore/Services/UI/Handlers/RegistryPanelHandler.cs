using NewCore.Services.UI.Registries;
using NewCore.ViewModels;
using NewCore.Views.UI;

namespace NewCore.Services.UI.Handlers
{
    public abstract class RegistryPanelHandler<TPanel, TViewModel> : PanelHandler<TPanel, TViewModel>
        where TPanel : PanelView<TViewModel>
        where TViewModel : IViewModel
    {
        private readonly IPanelRegistry _registry;

        protected RegistryPanelHandler(TPanel panel, TViewModel context, IUIContainerRoot uiRoots,
            IPanelRegistry registry) : base(panel, context, uiRoots) => _registry = registry;

        public override void Open()
        {
            _registry.Register(this);
            base.Open();
        }

        public override void Close()
        {
            _registry.Unregister(this);
            base.Close();
        }
    }
}