using NewCore.Services.UI;
using NewCore.ViewModels;

namespace NewCore.Views.UI
{
    public abstract class ScreenView<TViewModel> : PanelView<TViewModel>, IScreen where TViewModel : IViewModel
    {
    }
}