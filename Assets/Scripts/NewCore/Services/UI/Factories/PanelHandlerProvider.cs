using System.Collections.Generic;
using System.Linq;
using NewCore.Services.UI.Handlers;
using NewCore.ViewModels;
using NewCore.Views.UI;

namespace NewCore.Services.UI.Factories
{
    public sealed class PanelHandlerProvider : IPanelHandlerProvider
    {
        private readonly List<IPanelHandlerResolver> _resolvers;

        public PanelHandlerProvider(IEnumerable<IPanelHandlerResolver> resolvers) => _resolvers = resolvers.ToList();

        public IPanelHandler Provide(IPanel panel, IViewModel viewModel, IUIContainerRoot uiRoots)
        {
            var panelType = panel.GetType();
            var resolver = _resolvers.First(handlerResolver => handlerResolver.CanResolve(panelType));
            return resolver.Resolve(panel, viewModel, uiRoots);
        }
    }
}