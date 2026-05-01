using Modules.Presentation.Runtime.Contracts;
using Modules.Presentation.Runtime.Contracts.Registry;
using Modules.Presentation.Runtime.Panels;

namespace Modules.Presentation.Runtime.Registry
{
    public interface IPanelRegistryWriter : IPanelRegistry
    {
        void Register<TPresenter>(IPanelLifetimeHandle<TPresenter> handle)
            where TPresenter : PanelPresenter;

        void Unregister(IPanelLifetimeHandle handle);
    }
}