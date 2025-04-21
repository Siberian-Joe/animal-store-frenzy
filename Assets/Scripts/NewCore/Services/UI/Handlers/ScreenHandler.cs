using NewCore.ViewModels;
using NewCore.Views.UI;

namespace NewCore.Services.UI.Handlers
{
    public class ScreenHandler<TPanel, TViewModel> : BaseHandler<TPanel, TViewModel>
        where TPanel : PanelBinder<TViewModel>, IScreen
        where TViewModel : IViewModel
    {
        public ScreenHandler(TPanel panel, TViewModel context, PanelService owner)
            : base(panel, context, owner)
        {
        }

        public override void Open()
        {
            if (Owner.ActiveScreen != this)
            {
                Owner.ActiveScreen?.Close();
                Owner.ActiveScreen = this;
            }

            Panel.transform.SetParent(Owner.ScreensRoot, false);
            Panel.Open();
        }

        public override void Close()
        {
            base.Close();
            if (Owner.ActiveScreen == this)
                Owner.ActiveScreen = null;
        }
    }
}