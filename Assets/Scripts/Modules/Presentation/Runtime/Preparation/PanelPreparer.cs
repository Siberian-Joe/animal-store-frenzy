using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Modules.Presentation.Contracts.Catalog;
using Modules.Presentation.Contracts.Preparation;
using Modules.Presentation.Runtime.Handles;
using Modules.Presentation.Runtime.Lifecycle;
using Modules.Presentation.Runtime.Panels;
using Modules.Presentation.Runtime.Registry;
using Modules.Presentation.Runtime.Root;
using Modules.ResourceLoading.Contracts;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Modules.Presentation.Runtime.Preparation
{
    public sealed class PanelPreparer : IPanelPreparer
    {
        private readonly IPanelCatalog _catalog;
        private readonly IPanelRootAccess _panelRootAccess;
        private readonly IPanelPreparationScopeAccess _scopeAccess;
        private readonly IPanelRegistryWriter _registry;
        private readonly IResourceLoader _resourceLoader;

        public PanelPreparer(
            IPanelCatalog catalog,
            IPanelRootAccess panelRootAccess,
            IPanelPreparationScopeAccess scopeAccess,
            IPanelRegistryWriter registry,
            IResourceLoader resourceLoader)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _panelRootAccess = panelRootAccess ?? throw new ArgumentNullException(nameof(panelRootAccess));
            _scopeAccess = scopeAccess ?? throw new ArgumentNullException(nameof(scopeAccess));
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
            _resourceLoader = resourceLoader ?? throw new ArgumentNullException(nameof(resourceLoader));
        }

        public async UniTask PrepareAsync<TPresenter>(CancellationToken token = default)
            where TPresenter : PanelPresenter
        {
            token.ThrowIfCancellationRequested();

            if (_registry.TryGet<TPresenter>(out _))
                return;

            var panelRoot = _panelRootAccess.Root;
            var scope = _scopeAccess.Current;
            var factory = scope.InstanceFactory;

            var presenter = factory.CreatePresenter<TPresenter>();
            var prefabLocation = _catalog.GetPrefabLocation(presenter.PanelType);
            var prefabLease = await _resourceLoader.LoadAsync<GameObject>(prefabLocation, token);

            GameObject root = null;

            try
            {
                root = factory.CreatePanelRoot(prefabLease.Asset);
                root.SetActive(false);

                if (root.GetComponent(presenter.PanelType) is not Panel panel)
                    throw new InvalidOperationException(
                        $"Prefab '{prefabLease.Asset.name}' does not contain required panel '{presenter.PanelType.FullName}' on root.");

                var layer = panelRoot.GetLayerFor(panel);

                root.transform.SetParent(layer.CacheRoot, false);

                presenter.AttachTo(panel);

                var handle = new PanelHandle<TPresenter>(
                    layer,
                    _registry,
                    prefabLease,
                    root,
                    panel,
                    presenter);

                _registry.Register(handle);
                scope.LifetimeStore.Add(handle);
            }
            catch
            {
                if (root)
                    Object.Destroy(root);

                prefabLease.Dispose();
                throw;
            }
        }
    }
}