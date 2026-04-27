using Game.Presentation.Runtime.Layers;
using Game.Presentation.Runtime.Panels;

namespace Game.Presentation.Contracts
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