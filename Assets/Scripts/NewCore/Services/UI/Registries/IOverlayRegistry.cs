using System.Collections.Generic;
using NewCore.Services.UI.Handlers;

namespace NewCore.Services.UI.Registries
{
    public interface IOverlayRegistry
    {
        IReadOnlyCollection<IPanelHandler> ActiveOverlays { get; }

        void RegisterOverlay(IPanelHandler handler);
        void UnregisterOverlay(IPanelHandler handler);
    }
}