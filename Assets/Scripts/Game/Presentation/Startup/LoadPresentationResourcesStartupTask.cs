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
    public sealed class LoadPresentationResourcesStartupTask : IApplicationStartupTask, IDisposable
    {
        private readonly IResourceLoader _resourceLoader;
        private readonly AssetReference _resourcesConfigReference;
        private readonly IPanelCatalogInitializer _catalogInitializer;
        private readonly IPresentationRootInitializer _presentationRootInitializer;
        private readonly IPresentationRootAccess _presentationRootAccess;
        private readonly IPanelPreparationScopeAccess _scopeAccess;

        private IResourceLease<PresentationResourcesConfig> _configLease;

        public string Name => "Load presentation resources";

        public LoadPresentationResourcesStartupTask(
            IResourceLoader resourceLoader,
            AssetReference resourcesConfigReference,
            IPanelCatalogInitializer catalogInitializer,
            IPresentationRootInitializer presentationRootInitializer,
            IPresentationRootAccess presentationRootAccess,
            IPanelPreparationScopeAccess scopeAccess)
        {
            _resourceLoader = resourceLoader ?? throw new ArgumentNullException(nameof(resourceLoader));
            _resourcesConfigReference = resourcesConfigReference
                                        ?? throw new ArgumentNullException(nameof(resourcesConfigReference));
            _catalogInitializer = catalogInitializer ?? throw new ArgumentNullException(nameof(catalogInitializer));
            _presentationRootInitializer =
                presentationRootInitializer ?? throw new ArgumentNullException(nameof(presentationRootInitializer));
            _presentationRootAccess = presentationRootAccess ?? throw new ArgumentNullException(nameof(presentationRootAccess));
            _scopeAccess = scopeAccess ?? throw new ArgumentNullException(nameof(scopeAccess));
        }

        public async UniTask ExecuteAsync(CancellationToken token)
        {
            if (_configLease != null)
                return;

            var configLease = await _resourceLoader.LoadAsync<PresentationResourcesConfig>(
                _resourcesConfigReference,
                token);

            PanelRootResolution rootResolution = null;

            try
            {
                var config = configLease.Asset;
                ValidateConfig(config);

                rootResolution = await ResolvePanelRootAsync(config, token);

                var locations = await _resourceLoader.LocateAsync<GameObject>(
                    config.PanelPrefabsLabel,
                    token);

                var catalogEntries = await BuildCatalogAsync(
                    rootResolution.Root,
                    locations,
                    token);

                _catalogInitializer.Initialize(catalogEntries);

                if (rootResolution.CreatedByTask)
                {
                    _presentationRootInitializer.Initialize(
                        rootResolution.CreatedRoot,
                        rootResolution.PrefabLease);

                    rootResolution.ReleaseOwnershipToProvider();
                }

                _configLease = configLease;

                configLease = null;
            }
            catch
            {
                rootResolution?.Dispose();
                throw;
            }
            finally
            {
                configLease?.Dispose();
            }
        }

        public void Dispose()
        {
            _configLease?.Dispose();
            _configLease = null;
        }

        private async UniTask<PanelRootResolution> ResolvePanelRootAsync(
            PresentationResourcesConfig config,
            CancellationToken token)
        {
            if (_presentationRootInitializer.IsInitialized)
                return PanelRootResolution.Existing(_presentationRootAccess.Root);

            var rootPrefabLease = await _resourceLoader.LoadAsync<GameObject>(
                config.PanelRootPrefab,
                token);

            PresentationRoot presentationRoot = null;

            try
            {
                presentationRoot = _scopeAccess.Current.InstanceFactory
                    .CreateComponent<PresentationRoot>(rootPrefabLease.Asset);

                if (presentationRoot == false)
                    throw new InvalidOperationException(
                        $"Panel root prefab '{rootPrefabLease.Asset.name}' does not contain {nameof(PresentationRoot)}.");

                presentationRoot.transform.SetParent(null, false);
                Object.DontDestroyOnLoad(presentationRoot.gameObject);

                return PanelRootResolution.Created(presentationRoot, rootPrefabLease);
            }
            catch
            {
                if (presentationRoot)
                    Object.Destroy(presentationRoot.gameObject);

                rootPrefabLease.Dispose();

                throw;
            }
        }

        private async UniTask<IReadOnlyDictionary<Type, IResourceLocation>> BuildCatalogAsync(
            IPresentationRoot presentationRoot,
            IReadOnlyList<IResourceLocation> locations,
            CancellationToken token)
        {
            if (presentationRoot == null)
                throw new ArgumentNullException(nameof(presentationRoot));

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
                    throw new InvalidOperationException(
                        $"Discovered panel prefab '{prefab.name}' does not contain {nameof(Panel)} on the root.");

                var panelType = panel.GetType();

                _ = presentationRoot.GetLayerFor(panel);

                if (entries.TryAdd(panelType, location) == false)
                    throw new InvalidOperationException(
                        $"Duplicate discovered panel binding '{panelType.FullName}'.");
            }

            return entries;
        }

        private static void ValidateConfig(PresentationResourcesConfig config)
        {
            if (config == false)
                throw new ArgumentNullException(nameof(config));

            if (config.PanelRootPrefab == null || config.PanelRootPrefab.RuntimeKeyIsValid() == false)
                throw new InvalidOperationException(
                    $"{nameof(PresentationResourcesConfig)} requires a valid panel root prefab reference.");

            if (config.PanelPrefabsLabel == null || string.IsNullOrWhiteSpace(config.PanelPrefabsLabel.labelString))
                throw new InvalidOperationException(
                    $"{nameof(PresentationResourcesConfig)} requires a valid panel prefabs label.");
        }

        private sealed class PanelRootResolution : IDisposable
        {
            private PanelRootResolution(
                IPresentationRoot root,
                PresentationRoot createdRoot,
                IResourceLease<GameObject> prefabLease,
                bool createdByTask)
            {
                Root = root ?? throw new ArgumentNullException(nameof(root));
                CreatedRoot = createdRoot;
                PrefabLease = prefabLease;
                CreatedByTask = createdByTask;
            }

            public IPresentationRoot Root { get; }

            public PresentationRoot CreatedRoot { get; private set; }

            public IResourceLease<GameObject> PrefabLease { get; private set; }

            public bool CreatedByTask { get; private set; }

            public static PanelRootResolution Existing(IPresentationRoot root)
            {
                return new PanelRootResolution(
                    root,
                    createdRoot: null,
                    prefabLease: null,
                    createdByTask: false);
            }

            public static PanelRootResolution Created(
                PresentationRoot root,
                IResourceLease<GameObject> prefabLease)
            {
                if (root == false)
                    throw new ArgumentNullException(nameof(root));

                if (prefabLease == null)
                    throw new ArgumentNullException(nameof(prefabLease));

                return new PanelRootResolution(
                    root,
                    root,
                    prefabLease,
                    createdByTask: true);
            }

            public void ReleaseOwnershipToProvider()
            {
                CreatedRoot = null;
                PrefabLease = null;
                CreatedByTask = false;
            }

            public void Dispose()
            {
                if (CreatedRoot)
                    Object.Destroy(CreatedRoot.gameObject);

                CreatedRoot = null;

                PrefabLease?.Dispose();
                PrefabLease = null;

                CreatedByTask = false;
            }
        }
    }
}