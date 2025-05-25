using NewCore.ViewModels;
using NewCore.Views.UI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NewCore.Services.UI.Handlers
{
    public abstract class PanelHandler<TPanel, TViewModel> : IPanelHandler<TViewModel>
        where TPanel : PanelView<TViewModel>
        where TViewModel : IViewModel
    {
        public TViewModel Context { get; }

        protected readonly TPanel Panel;
        protected readonly IUIContainerRoot UIRoots;

        protected abstract Transform Container { get; }

        protected PanelHandler(TPanel panel, TViewModel context, IUIContainerRoot uiRoots)
        {
            Panel = panel;
            Context = context;
            UIRoots = uiRoots;
        }

        public virtual void Open()
        {
            if (Panel == null)
                return;

            Panel.transform.SetParent(Container, false);
            Panel.Open();
        }

        public virtual void Close()
        {
            if (Panel == null)
                return;

            Panel.Close();
            Panel.transform.SetParent(UIRoots.PanelsCache, false);
        }

        public virtual void Dispose()
        {
            Close();

            if (Panel == null)
                return;

            Panel.Dispose();
            Object.Destroy(Panel.gameObject);
        }
    }
}