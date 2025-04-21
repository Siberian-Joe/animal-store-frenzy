using NewCore.ViewModels;
using NewCore.Views.UI;

namespace NewCore.Services.UI.Handlers
{
    public class OverlayHandler<TPanel, TViewModel> : BaseHandler<TPanel, TViewModel>
        where TPanel : PanelBinder<TViewModel>, IOverlay
        where TViewModel : IViewModel
    {
        public OverlayHandler(TPanel panel, TViewModel context, PanelService owner)
            : base(panel, context, owner)
        {
        }

        public override void Open()
        {
            Owner.OverlayStack.Push(this);
            Panel.transform.SetParent(Owner.OverlayRoot, false);
            Panel.transform.SetSiblingIndex((Panel as IOverlay).Order);
            Panel.Open();
        }

        public override void Close()
        {
            if (Owner.OverlayStack.Count > 0 &&
                Owner.OverlayStack.Peek() == this)
            {
                Owner.OverlayStack.Pop();
            }

            base.Close();
        }
    }
}