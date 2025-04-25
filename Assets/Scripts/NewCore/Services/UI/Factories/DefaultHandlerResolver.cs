using NewCore.Services.UI.Handlers;
using Zenject;

namespace NewCore.Services.UI.Factories
{
    public sealed class DefaultHandlerResolver : PanelHandlerResolver
    {
        public DefaultHandlerResolver(DiContainer container) : base(container, typeof(IPanel), typeof(PanelHandler<,>))
        {
        }
    }
}