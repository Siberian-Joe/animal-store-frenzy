using Game.Presentation.Runtime.Panels;

namespace Game.Presentation.Contracts.Navigation
{
    public interface IPanelNavigator
    {
        bool CanGoBack { get; }

        void Open<TPresenter>()
            where TPresenter : PanelPresenter;

        bool Back();

        void Close<TPresenter>()
            where TPresenter : PanelPresenter;

        void Clear();
    }
}