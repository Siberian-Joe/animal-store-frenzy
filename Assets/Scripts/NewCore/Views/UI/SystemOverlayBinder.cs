using NewCore.Services.UI;
using NewCore.ViewModels;

namespace NewCore.Views.UI
{
    public abstract class SystemOverlayBinder<TViewModel> : OverlayBinder<TViewModel>, ISystemOverlay
        where TViewModel : IViewModel
    {
        public override int Order => 0;
    }
}