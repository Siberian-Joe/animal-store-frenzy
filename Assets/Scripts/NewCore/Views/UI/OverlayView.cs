using NewCore.Services.UI;
using NewCore.ViewModels;

namespace NewCore.Views.UI
{
    public abstract class OverlayView<TViewModel> : PanelView<TViewModel>, IOverlay where TViewModel : IViewModel
    {
        public abstract int Order { get; }
    }
}