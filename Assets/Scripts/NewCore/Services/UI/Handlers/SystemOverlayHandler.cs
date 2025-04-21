using NewCore.ViewModels;
using NewCore.Views.UI;

namespace NewCore.Services.UI.Handlers
{
    public class SystemOverlayHandler<TPanel, TViewModel> : BaseHandler<TPanel, TViewModel>
        where TPanel : PanelBinder<TViewModel>, ISystemOverlay
        where TViewModel : IViewModel
    {
        public SystemOverlayHandler(TPanel panel, TViewModel context, PanelService owner)
            : base(panel, context, owner)
        {
        }

        public override void Open()
        {
            if (Owner.ActiveSystemOverlay != this)
            {
                Owner.ActiveSystemOverlay?.Close();
                Owner.ActiveSystemOverlay = this;
            }

            Panel.transform.SetParent(Owner.SystemOverlayRoot, false);
            Panel.Open();
        }

        public override void Close()
        {
            base.Close();
            if (Owner.ActiveSystemOverlay == this)
                Owner.ActiveSystemOverlay = null;
        }
    }
}