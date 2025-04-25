using NewCore.Services.UI.Handlers;

namespace NewCore.Services.UI.Registries
{
    public interface ISystemOverlayRegistry
    {
        IPanelHandler ActiveSystemOverlay { get; }

        void RegisterSystemOverlay(IPanelHandler handler);
    }
}