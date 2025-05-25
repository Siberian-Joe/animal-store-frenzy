using NewCore.Services.UI;
using NewCore.ViewModels;

namespace NewCore.Views.UI
{
    public abstract class SystemOverlayView<TViewModel> : PanelView<TViewModel>, ISystemOverlay
        where TViewModel : IViewModel
    {
    }
}