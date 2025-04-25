using NewCore.Services.UI.Handlers;
using Zenject;

namespace NewCore.Services.UI.Factories
{
    public sealed class OverlayHandlerResolver : PanelHandlerResolver
    {
        public OverlayHandlerResolver(DiContainer container) : base(container, typeof(IOverlay), typeof(OverlayHandler<,>))
        {
        }
    }
}