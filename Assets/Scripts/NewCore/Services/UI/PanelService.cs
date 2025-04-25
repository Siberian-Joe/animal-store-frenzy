using Cysharp.Threading.Tasks;
using NewCore.Data;
using NewCore.Factories;
using NewCore.Services.ResourceLoaders;
using NewCore.Services.UI.Factories;
using NewCore.Services.UI.Handlers;
using NewCore.Services.UI.Registries;
using NewCore.ViewModels;
using NewCore.Views.UI;
using Object = UnityEngine.Object;

namespace NewCore.Services.UI
{
    public sealed class PanelService : IPanelService
    {
        private readonly IResourceLoader _resourceLoader;
        private readonly IViewModelFactory _viewModelFactory;
        private readonly IPanelCache _cache;
        private readonly IPanelHandlerProvider _handlerProvider;

        private IUIContainerRoot _roots;

        public PanelService(IResourceLoader resourceLoader, IViewModelFactory viewModelFactory, IPanelCache cache,
            IPanelHandlerProvider handlerProvider)
        {
            _resourceLoader = resourceLoader;
            _viewModelFactory = viewModelFactory;
            _cache = cache;
            _handlerProvider = handlerProvider;
        }

        public async UniTask<IPanelHandler<TViewModel>> LoadPanelAsync<TPanel, TProxy, TViewModel>()
            where TPanel : PanelBinder<TViewModel>
            where TProxy : IProxy, new()
            where TViewModel : class, IViewModel
        {
            if (_roots == null)
            {
                var roots = await _resourceLoader.InstantiateResourceAsync<UIContainerRoot>();
                _roots = roots;

                Object.DontDestroyOnLoad(roots.gameObject);
            }

            var key = typeof(TPanel);
            if (_cache.TryGetHandler(key, out var existing))
                return (IPanelHandler<TViewModel>)existing;

            var view = await _resourceLoader.InstantiateResourceAsync<TPanel>();
            var viewModel = _viewModelFactory.Create<TProxy, TViewModel>(new TProxy());
            view.Bind(viewModel);
            view.Close();

            var handler = (IPanelHandler<TViewModel>)_handlerProvider.Provide(view, viewModel, _roots);

            _cache.StoreHandler(key, handler);
            return handler;
        }
    }
}