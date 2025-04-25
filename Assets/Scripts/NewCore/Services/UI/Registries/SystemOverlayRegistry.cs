using NewCore.Services.UI.Handlers;

namespace NewCore.Services.UI.Registries
{
    public class SystemOverlayRegistry : SingleActiveRegistry, ISystemOverlayRegistry
    {
        public IPanelHandler ActiveSystemOverlay => Active;

        public void RegisterSystemOverlay(IPanelHandler handler) => Register(handler);
    }
}