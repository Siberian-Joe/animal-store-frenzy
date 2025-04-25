using NewCore.Services.UI.Handlers;

namespace NewCore.Services.UI.Registries
{
    public interface IScreenRegistry
    {
        IPanelHandler ActiveScreen { get; }

        void RegisterScreen(IPanelHandler handler);
    }
}