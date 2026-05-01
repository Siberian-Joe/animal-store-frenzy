using Modules.Presentation.Runtime.Panels;

namespace Modules.Presentation.Runtime.Contracts.Registry
{
    public interface IPanelRegistry
    {
        IPanelHandle<TPresenter> Get<TPresenter>()
            where TPresenter : PanelPresenter;

        bool TryGet<TPresenter>(out IPanelHandle<TPresenter> handle)
            where TPresenter : PanelPresenter;
    }
}