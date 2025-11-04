using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using NewCore.Data;
using NewCore.Domain;
using NewCore.Factories;
using NewCore.Modules.Interaction;
using NewCore.Services.ResourceLoaders;
using NewCore.Services.UI.Handlers;
using NewCore.Services.UI.Handlers.Decorators;
using NewCore.Services.UI.Registries;
using NewCore.Views.UI;
using IViewModel = NewCore.ViewModels.IViewModel;

namespace NewCore.Services.UI.Factories
{
    public sealed class PanelHandlerFactory : IPanelHandlerFactory
    {
        private readonly IResourceLoader _resourceLoader;
        private readonly IViewModelFactory _viewModelFactory;
        private readonly IPanelCache _cache;
        private readonly IEnumerable<IPanelHandlerResolver> _resolvers;
        private readonly IProxyFactory _proxyFactory;

        public PanelHandlerFactory(
            IResourceLoader resourceLoader,
            IViewModelFactory viewModelFactory,
            IPanelCache cache,
            IEnumerable<IPanelHandlerResolver> resolvers,
            IProxyFactory proxyFactory)
        {
            _resourceLoader = resourceLoader;
            _viewModelFactory = viewModelFactory;
            _cache = cache;
            _resolvers = resolvers;
            _proxyFactory = proxyFactory;
        }

        public async UniTask<IPanelHandler<TViewModel>> CreateAsync<TPanel, TModel, TProxy, TViewModel>(
            UIContainerRoot roots,
            CancellationToken cancellationToken = default)
            where TPanel : PanelView<TViewModel>
            where TModel : IModel, new()
            where TProxy : IProxy
            where TViewModel : class, IViewModel
        {
            var panelType = typeof(TPanel);
            if (_cache.TryGetHandler(panelType, out var existing))
                return (IPanelHandler<TViewModel>)existing;

            var view = await _resourceLoader.InstantiateResourceAsync<TPanel>(cancellationToken: cancellationToken);
            var viewModel = _viewModelFactory.Create<TProxy, TViewModel>(_proxyFactory.Create<TProxy>(new TModel()));

            view.Bind(viewModel);
            view.Close();

            var resolver = _resolvers.First(handlerResolver => handlerResolver.CanResolve(typeof(TPanel)));
            var handler = resolver.Resolve(view, viewModel, roots);

            _cache.StoreHandler(panelType, handler);
            return new CachingDecorator<TViewModel>(handler, _cache, typeof(TPanel));
        }
    }
}