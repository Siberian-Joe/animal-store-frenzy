using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NewCore.Data;
using NewCore.Factories;
using NewCore.Services.ResourceLoaders;
using NewCore.Services.UI.Handlers;
using NewCore.ViewModels;
using NewCore.Views.UI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NewCore.Services.UI
{
    public sealed class PanelService : IPanelService
    {
        private readonly IResourceLoader _resourceLoader;
        private readonly IViewModelFactory _viewModelFactory;

        private readonly Dictionary<Type, IPanelHandler> _cache = new();

        public Transform ScreensRoot { get; private set; }
        public Transform OverlayRoot { get; private set; }
        public Transform SystemOverlayRoot { get; private set; }
        public Transform CacheRoot { get; private set; }

        private bool _initialized;

        public IPanelHandler ActiveScreen { get; set; }
        public IPanelHandler ActiveSystemOverlay { get; set; }
        public Stack<IPanelHandler> OverlayStack { get; } = new();

        public PanelService(IResourceLoader resourceLoader, IViewModelFactory viewModelFactory)
        {
            _resourceLoader = resourceLoader;
            _viewModelFactory = viewModelFactory;
        }

        public async UniTask<IPanelHandler<TViewModel>> LoadPanelAsync<TPanel, TProxy, TViewModel>()
            where TPanel : PanelBinder<TViewModel>
            where TProxy : IProxy, new()
            where TViewModel : class, IViewModel
        {
            await EnsureRoots();

            var key = typeof(TPanel);
            if (_cache.TryGetValue(key, out var existing))
                return (IPanelHandler<TViewModel>)existing;

            var view = await _resourceLoader.InstantiateResourceAsync<TPanel>();
            var vm = _viewModelFactory.Create<TProxy, TViewModel>(new TProxy());
            view.Bind(vm);
            view.Close();

            var wrapperOpenGeneric = view switch
            {
                IScreen => typeof(ScreenHandler<,>),
                ISystemOverlay => typeof(SystemOverlayHandler<,>),
                IOverlay => typeof(OverlayHandler<,>),
                _ => typeof(BaseHandler<,>)
            };

            var wrapperType = wrapperOpenGeneric.MakeGenericType(key, typeof(TViewModel));

            var wrapper = (IPanelHandler<TViewModel>)Activator.CreateInstance(
                wrapperType,
                view,
                vm,
                this
            );

            _cache[key] = wrapper;
            return wrapper;
        }

        private async UniTask EnsureRoots()
        {
            if (_initialized)
                return;

            var uiRoot = await _resourceLoader.InstantiateResourceAsync<UIContainerRoot>();
            Object.DontDestroyOnLoad(uiRoot.gameObject);

            ScreensRoot = uiRoot.ScreensContainer;
            OverlayRoot = uiRoot.OverlayContainer;
            SystemOverlayRoot = uiRoot.SystemOverlayContainer;
            CacheRoot = uiRoot.PanelsCache;

            _initialized = true;
        }
    }
}