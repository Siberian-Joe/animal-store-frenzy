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
        private readonly IPresentationRootAccess _presentationRootAccess;
        private readonly IPanelPreparationScopeAccess _scopeAccess;
        private readonly IPanelRegistryWriter _registry;
        private readonly IResourceLoader _resourceLoader;

        public PanelPreparer(
            IPanelCatalog catalog,
            IPresentationRootAccess presentationRootAccess,
            IPanelPreparationScopeAccess scopeAccess,
            IPanelRegistryWriter registry,
            IResourceLoader resourceLoader)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _presentationRootAccess = presentationRootAccess ?? throw new ArgumentNullException(nameof(presentationRootAccess));
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

            var panelRoot = _presentationRootAccess.Root;
            var scope = _scopeAccess.Current;
            var factory = scope.InstanceFactory;

            var presenter = factory.CreatePresenter<TPresenter>();

            if (panelRoot is PresentationRoot concreteRoot &&
                concreteRoot.TryTakePreplacedPanel(presenter.PanelType, out var preplacedPanel))
            {
                PreparePreplacedPanel(
                    scope,
                    presenter,
                    preplacedPanel);

                return;
            }

            await PrepareAddressablePanel(
                scope,
                factory,
                panelRoot,
                presenter,
                token);
        }

        private void PreparePreplacedPanel<TPresenter>(
            IPanelPreparationScope scope,
            TPresenter presenter,
            Panel panel)
            where TPresenter : PanelPresenter
        {
            if (scope == null)
                throw new ArgumentNullException(nameof(scope));

            if (presenter == null)
                throw new ArgumentNullException(nameof(presenter));

            if (panel == false)
                throw new ArgumentNullException(nameof(panel));

            var panelRoot = _presentationRootAccess.Root;
            var layer = panelRoot.GetLayerFor(panel);
            var root = panel.gameObject;

            presenter.AttachTo(panel);

            var handle = new PanelHandle<TPresenter>(
                layer,
                _registry,
                prefabLease: null,
                root,
                panel,
                presenter);

            _registry.Register(handle);
            scope.LifetimeStore.Add(handle);
        }

        private async UniTask PrepareAddressablePanel<TPresenter>(
            IPanelPreparationScope scope,
            IPanelInstanceFactory factory,
            IPresentationRoot presentationRoot,
            TPresenter presenter,
            CancellationToken token)
            where TPresenter : PanelPresenter
        {
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

                var layer = presentationRoot.GetLayerFor(panel);

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

                root = null;
                prefabLease = null;
            }
            catch
            {
                if (root)
                    Object.Destroy(root);

                throw;
            }
            finally
            {
                prefabLease?.Dispose();
            }
        }
    }
}