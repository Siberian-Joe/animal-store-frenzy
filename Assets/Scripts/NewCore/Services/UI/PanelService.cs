using System.Threading;
using Cysharp.Threading.Tasks;
using NewCore.Data;
using NewCore.Domain;
using NewCore.Services.ResourceLoaders;
using NewCore.Services.UI.Factories;
using NewCore.Services.UI.Handlers;
using NewCore.Services.UI.Registries;
using NewCore.ViewModels;
using NewCore.Views.UI;
using UnityEngine;

namespace NewCore.Services.UI
{
    public sealed class PanelService : IPanelService
    {
        private readonly IPanelCache _cache;
        private readonly IPanelHandlerFactory _handlerFactory;

        private readonly AsyncLazy<UIContainerRoot> _lazyRoots;

        public PanelService(IResourceLoader resourceLoader, IPanelHandlerFactory handlerFactory)
        {
            _handlerFactory = handlerFactory;

            _lazyRoots = UniTask.Lazy(async () =>
            {
                var roots = await resourceLoader.InstantiateResourceAsync<UIContainerRoot>();
                Object.DontDestroyOnLoad(roots.gameObject);
                return roots;
            });
        }

        public async UniTask<IPanelHandler<TViewModel>> LoadPanelAsync<TPanel, TModel, TProxy, TViewModel>(
            CancellationToken cancellationToken = default)
            where TPanel : PanelView<TViewModel>
            where TModel : IModel, new()
            where TProxy : IProxy
            where TViewModel : class, IViewModel
        {
            var roots = await _lazyRoots;
            var handler = await _handlerFactory.CreateAsync<TPanel, TModel, TProxy, TViewModel>(roots, cancellationToken);

            return handler;
        }
    }
}