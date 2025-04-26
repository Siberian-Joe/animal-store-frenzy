using NewCore.Services.UI.Handlers;

namespace NewCore.Services.UI.Registries
{
    public interface IPanelRegistry
    {
        void Register(IPanelHandler handler);
        void Unregister(IPanelHandler handler);
    }
}