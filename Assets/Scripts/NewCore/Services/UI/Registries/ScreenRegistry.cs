using NewCore.Services.UI.Handlers;

namespace NewCore.Services.UI.Registries
{
    public class ScreenRegistry : SingleActiveRegistry, IScreenRegistry
    {
        public IPanelHandler ActiveScreen => Active;

        public void RegisterScreen(IPanelHandler handler) => Register(handler);
    }
}