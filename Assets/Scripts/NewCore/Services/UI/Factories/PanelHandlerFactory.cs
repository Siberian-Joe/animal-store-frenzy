using System.Collections.Generic;
using System.Linq;
using NewCore.Services.UI.Handlers;
using NewCore.Services.UI.Handlers.Decorators;
using NewCore.Services.UI.Registries;
using NewCore.ViewModels;
using NewCore.Views.UI;

namespace NewCore.Services.UI.Factories
{
    public sealed class PanelHandlerFactory : IPanelHandlerFactory
    {
        private readonly List<IPanelHandlerResolver> _resolvers;
        private readonly IPanelCache _panelCache;

        public PanelHandlerFactory(IEnumerable<IPanelHandlerResolver> resolvers, IPanelCache panelCache)
        {
            _resolvers = resolvers.ToList();
            _panelCache = panelCache;
        }

        public IPanelHandler<TViewModel> Create<TPanel, TViewModel>(TPanel panel, TViewModel viewModel,
            IUIContainerRoot uiRoots) where TPanel : PanelBinder<TViewModel> where TViewModel : class, IViewModel
        {
            var resolver = _resolvers.First(handlerResolver => handlerResolver.CanResolve(typeof(TPanel)));
            var baseHandler = resolver.Resolve<TPanel, TViewModel>(panel, viewModel, uiRoots);

            return new CachingDecorator<TViewModel>(baseHandler, _panelCache, typeof(TPanel));
        }
    }
}