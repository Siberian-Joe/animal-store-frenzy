using NewCore.Data.UI;
using NewCore.Domain.UI;

namespace NewCore.ViewModels.UI
{
    public class LoadingSystemOverlayViewModel : ViewModel<LoadingSystemOverlayModel, LoadingSystemOverlayProxy>
    {
        public LoadingSystemOverlayViewModel(LoadingSystemOverlayProxy proxy) : base(proxy)
        {
        }
    }
}