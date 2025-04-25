using NewCore.Services.UI.Handlers;
using Zenject;

namespace NewCore.Services.UI.Factories
{
    public sealed class SystemOverlayHandlerResolver : PanelHandlerResolver
    {
        public SystemOverlayHandlerResolver(DiContainer container) : base(container, typeof(ISystemOverlay),
            typeof(SystemOverlayHandler<,>))
        {
        }
    }
}