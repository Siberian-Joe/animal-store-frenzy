using NewCore.Services.UI.Handlers;

namespace NewCore.Services.UI.Registries
{
    public interface ISingleActiveRegistry : IPanelRegistry
    {
        IPanelHandler Active { get; }
    }
}