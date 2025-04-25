using NewCore.Services.UI.Handlers;
using Zenject;

namespace NewCore.Services.UI.Factories
{
    public sealed class ScreenHandlerResolver : PanelHandlerResolver
    {
        public ScreenHandlerResolver(DiContainer container) : base(container, typeof(IScreen), typeof(ScreenHandler<,>))
        {
        }
    }
}