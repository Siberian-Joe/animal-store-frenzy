using Game.Presentation.Contracts;
using Game.Presentation.Contracts.Registry;
using Game.Presentation.Runtime.Panels;

namespace Game.Presentation.Runtime.Registry
{
    public interface IPanelRegistryWriter : IPanelRegistry
    {
        void Register<TPresenter>(IPanelLifetimeHandle<TPresenter> handle)
            where TPresenter : PanelPresenter;

        void Unregister(IPanelLifetimeHandle handle);
    }
}