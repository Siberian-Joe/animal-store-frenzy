using Modules.Presentation.Runtime.Layers;
using Modules.Presentation.Runtime.Panels;

namespace Modules.Presentation.Runtime.Contracts
{
    public interface IPanelHandle
    {
        IPanelLayer Layer { get; }

        bool IsOpen { get; }

        void Open();

        void Close();
    }

    public interface IPanelHandle<out TPresenter> : IPanelHandle
        where TPresenter : PanelPresenter
    {
        TPresenter Presenter { get; }
    }
}