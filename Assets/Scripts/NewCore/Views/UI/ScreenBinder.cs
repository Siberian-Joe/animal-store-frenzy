using NewCore.Services.UI;
using NewCore.ViewModels;

namespace NewCore.Views.UI
{
    public abstract class ScreenBinder<TViewModel> : PanelBinder<TViewModel>, IScreen where TViewModel : IViewModel
    {
    }
}