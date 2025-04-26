using System.Linq;
using Cysharp.Threading.Tasks;
using NewCore.Data;
using NewCore.Factories;
using NewCore.Services.ResourceLoaders;
using NewCore.Services.UI.Handlers;
using NewCore.Services.UI.Handlers.Decorators;
using NewCore.Services.UI.Registries;
using NewCore.ViewModels;
using NewCore.Views.UI;

namespace NewCore.Services.UI.Factories
{
    public sealed class PanelHandlerFactory : IPanelHandlerFactory
    {
        private readonly IResourceLoader _resourceLoader;
        private readonly IViewModelFactory _viewModelFactory;
        private readonly IPanelCache _cache;
        private readonly IPanelHandlerResolver[] _resolvers;

        public PanelHandlerFactory(IResourceLoader resourceLoader, IViewModelFactory viewModelFactory,
            IPanelCache cache, IPanelHandlerResolver[] resolvers)
        {
            _resourceLoader = resourceLoader;
            _viewModelFactory = viewModelFactory;
            _cache = cache;
            _resolvers = resolvers;
        }

        public async UniTask<IPanelHandler<TViewModel>> CreateAsync<TPanel, TProxy, TViewModel>(UIContainerRoot roots)
            where TPanel : PanelBinder<TViewModel>
            where TProxy : IProxy, new()
            where TViewModel : class, IViewModel
        {
            var panelType = typeof(TPanel);
            if (_cache.TryGetHandler(panelType, out var existing))
                return (IPanelHandler<TViewModel>)existing;

            var view = await _resourceLoader.InstantiateResourceAsync<TPanel>();
            var viewModel = _viewModelFactory.Create<TProxy, TViewModel>(new TProxy());

            view.Bind(viewModel);
            view.Close();

            var resolver = _resolvers.First(handlerResolver => handlerResolver.CanResolve(typeof(TPanel)));
            var handler = resolver.Resolve(view, viewModel, roots);

            _cache.StoreHandler(panelType, handler);
            return new CachingDecorator<TViewModel>(handler, _cache, typeof(TPanel));
        }
    }
}