using Game.Presentation.Runtime.Handles;
using Game.Presentation.Runtime.Panels;
using UnityEngine;

namespace Game.Presentation.Runtime.Layers
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