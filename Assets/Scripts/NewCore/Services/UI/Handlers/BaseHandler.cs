using NewCore.ViewModels;
using NewCore.Views.UI;
using UnityEngine;

namespace NewCore.Services.UI.Handlers
{
    public class BaseHandler<TPanel, TViewModel> : IPanelHandler<TViewModel>
        where TPanel : PanelBinder<TViewModel>
        where TViewModel : IViewModel
    {
        protected readonly TPanel Panel;
        protected readonly PanelService Owner;

        public TViewModel Context { get; }

        public BaseHandler(TPanel panel, TViewModel context, PanelService owner)
        {
            Panel = panel;
            Context = context;
            Owner = owner;
        }

        public virtual void Open()
        {
            Panel.transform.SetParent(Owner.CacheRoot, false);
            Panel.Open();
        }

        public virtual void Close()
        {
            Panel.Close();
            Panel.transform.SetParent(Owner.CacheRoot, false);
        }

        public void Dispose()
        {
            Panel.Dispose();
            Object.Destroy(Panel.gameObject);
        }
    }
}