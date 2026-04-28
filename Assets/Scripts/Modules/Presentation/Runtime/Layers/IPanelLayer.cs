using Modules.Presentation.Runtime.Handles;
using Modules.Presentation.Runtime.Panels;
using UnityEngine;

namespace Modules.Presentation.Runtime.Layers
{
    public interface IPanelLayer
    {
        Transform ContentRoot { get; }

        Transform CacheRoot { get; }

        IPanelLayer NavigationParent { get; }

        bool RecordOpenedPanelsInNavigationHistory { get; }

        bool CanHandle(Panel panel);

        bool IsNavigationDescendantOf(IPanelLayer layer);

        void Open(IPanelRuntimeHandle handle);

        void Close(IPanelRuntimeHandle handle);

        void Release(IPanelRuntimeHandle handle);
    }
}