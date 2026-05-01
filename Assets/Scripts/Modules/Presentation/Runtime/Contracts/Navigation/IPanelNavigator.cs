using Modules.Presentation.Runtime.Panels;

namespace Modules.Presentation.Runtime.Contracts.Navigation
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