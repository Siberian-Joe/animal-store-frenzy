using NewCore.Services.UI;
using NewCore.ViewModels;

namespace NewCore.Views.UI
{
    public abstract class OverlayBinder<TViewModel> : PanelBinder<TViewModel>, IOverlay where TViewModel : IViewModel
    {
        public abstract int Order { get; }
    }
}