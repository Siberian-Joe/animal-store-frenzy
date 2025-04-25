using NewCore.Services.UI;
using NewCore.ViewModels;

namespace NewCore.Views.UI
{
    public abstract class SystemOverlayBinder<TViewModel> : PanelBinder<TViewModel>, ISystemOverlay
        where TViewModel : IViewModel
    {
    }
}