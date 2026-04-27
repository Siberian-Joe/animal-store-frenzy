using Game.Presentation.Runtime.Panels;

namespace Game.Presentation.Contracts.Registry
{
    public interface IPanelRegistry
    {
        IPanelHandle<TPresenter> Get<TPresenter>()
            where TPresenter : PanelPresenter;

        bool TryGet<TPresenter>(out IPanelHandle<TPresenter> handle)
            where TPresenter : PanelPresenter;
    }
}