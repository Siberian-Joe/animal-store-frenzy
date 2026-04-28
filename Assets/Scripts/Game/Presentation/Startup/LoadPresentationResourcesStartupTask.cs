using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Modules.Presentation.Configuration;
using Modules.Presentation.Runtime.Catalog;
using Modules.Presentation.Runtime.Panels;
using Modules.Presentation.Runtime.Preparation;
using Modules.Presentation.Runtime.Root;
using Modules.ResourceLoading.Contracts;
using Modules.Startup.Contracts;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using Object = UnityEngine.Object;

namespace Game.Presentation.Startup
{
    [Startup(StartupPhase.Foundation, Order = -1100)]
    public sealed class LoadPresentationResourcesStartupTask : IStartupTask
    {
        private readonly IResourceLoader _resourceLoader;
        private readonly AssetReference _resourcesConfigReference;
        private readonly IPanelCatalogInitializer _catalogInitializer;
        private readonly IPanelRootInitializer _panelRootInitializer;
        private readonly IPanelPreparationScopeAccess _scopeAccess;

        public string Name => "Load presentation resources";

        public LoadPresentationResourcesStartupTask(
            IResourceLoader resourceLoader,
            AssetReference resourcesConfigReference,
            IPanelCatalogInitializer catalogInitializer,
            IPanelRootInitializer panelRootInitializer,
            IPanelPreparationScopeAccess scopeAccess)
        {
            _resourceLoader = resourceLoader ?? throw new ArgumentNullException(nameof(resourceLoader));
            _resourcesConfigReference = resourcesConfigReference
                                        ?? throw new ArgumentNullException(nameof(resourcesConfigReference));
            _catalogInitializer = catalogInitializer ?? throw new ArgumentNullException(nameof(catalogInitializer));
            _panelRootInitializer =
                panelRootInitializer ?? throw new ArgumentNullException(nameof(panelRootInitializer));
            _scopeAccess = scopeAccess ?? throw new ArgumentNullException(nameof(scopeAccess));
        }

        public async UniTask ExecuteAsync(CancellationToken token)
        {
            using var configLease =
                await _resourceLoader.LoadAsync<PresentationResourcesConfig>(_resourcesConfigReference, token);

            var config = configLease.Asset;

            ValidateConfig(config);

            IResourceLease<GameObject> rootPrefabLease = null;
            PanelRoot panelRoot = null;

            try
            {
                rootPrefabLease = await _resourceLoader.LoadAsync<GameObject>(config.PanelRootPrefab, token);

                panelRoot = _scopeAccess.Current.InstanceFactory
                    .CreateComponent<PanelRoot>(rootPrefabLease.Asset);

                if (panelRoot == false)
                {
                    throw new InvalidOperationException(
                        $"Panel root prefab '{rootPrefabLease.Asset.name}' does not contain {nameof(PanelRoot)}.");
                }

                panelRoot.transform.SetParent(null, false);

                var locations = await _resourceLoader.LocateAsync<GameObject>(
                    config.PanelPrefabsLabel,
                    token);

                var catalogEntries = await BuildCatalogAsync(panelRoot, locations, token);

                _catalogInitializer.Initialize(catalogEntries);
                _panelRootInitializer.Initialize(panelRoot, rootPrefabLease);

                panelRoot = null;
                rootPrefabLease = null;
            }
            catch
            {
                if (panelRoot)
                    Object.Destroy(panelRoot.gameObject);

                rootPrefabLease?.Dispose();
                throw;
            }
        }

        private async UniTask<IReadOnlyDictionary<Type, IResourceLocation>> BuildCatalogAsync(
            IPanelRoot panelRoot,
            IReadOnlyList<IResourceLocation> locations,
            CancellationToken token)
        {
            if (panelRoot == null)
                throw new ArgumentNullException(nameof(panelRoot));

            if (locations == null)
                throw new ArgumentNullException(nameof(locations));

            var entries = new Dictionary<Type, IResourceLocation>();

            foreach (var location in locations)
            {
                token.ThrowIfCancellationRequested();

                using var prefabLease = await _resourceLoader.LoadAsync<GameObject>(location, token);
                var prefab = prefabLease.Asset;

                var panel = prefab.GetComponent<Panel>();
                if (panel == false)
                {
                    throw new InvalidOperationException(
                        $"Discovered panel prefab '{prefab.name}' does not contain {nameof(Panel)} on the root.");
                }

                var panelType = panel.GetType();

                _ = panelRoot.GetLayerFor(panel);

                if (entries.TryAdd(panelType, location) == false)
                {
                    throw new InvalidOperationException(
                        $"Duplicate discovered panel binding '{panelType.FullName}'.");
                }
            }

            return entries;
        }

        private static void ValidateConfig(PresentationResourcesConfig config)
        {
            if (config == false)
                throw new ArgumentNullException(nameof(config));

            if (config.PanelRootPrefab == null || config.PanelRootPrefab.RuntimeKeyIsValid() == false)
            {
                throw new InvalidOperationException(
                    $"{nameof(PresentationResourcesConfig)} requires a valid panel root prefab reference.");
            }

            if (config.PanelPrefabsLabel == null || string.IsNullOrWhiteSpace(config.PanelPrefabsLabel.labelString))
            {
                throw new InvalidOperationException(
                    $"{nameof(PresentationResourcesConfig)} requires a valid panel prefabs label.");
            }
        }
    }
}