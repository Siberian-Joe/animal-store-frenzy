using NewCore.ViewModels;
using NewCore.Views.UI;
using Object = UnityEngine.Object;

namespace NewCore.Services.UI.Handlers
{
    public class PanelHandler<TPanel, TViewModel> : IPanelHandler<TViewModel>
        where TPanel : PanelBinder<TViewModel>
        where TViewModel : IViewModel
    {
        protected readonly TPanel Panel;
        public TViewModel Context { get; }
        protected readonly IUIContainerRoot UIRoots;

        public PanelHandler(TPanel panel, TViewModel context, IUIContainerRoot uiRoots)
        {
            Panel = panel;
            Context = context;
            UIRoots = uiRoots;
        }

        public virtual void Open()
        {
            Panel.transform.SetParent(UIRoots.PanelsCache, false);
            Panel.Open();
        }

        public virtual void Close()
        {
            Panel.Close();
            Panel.transform.SetParent(UIRoots.PanelsCache, false);
        }

        public void Dispose()
        {
            Panel.Dispose();
            Object.Destroy(Panel.gameObject);
        }
    }
}