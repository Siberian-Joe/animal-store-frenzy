using NewCore.Services.UI;
using NewCore.ViewModels;

namespace NewCore.Views.UI
{
    public abstract class PanelBinder<TViewModel> : Binder<TViewModel>, IPanel
        where TViewModel : IViewModel
    {
        public virtual void Open() => gameObject.SetActive(true);

        public virtual void Close() => gameObject.SetActive(false);
    }
}