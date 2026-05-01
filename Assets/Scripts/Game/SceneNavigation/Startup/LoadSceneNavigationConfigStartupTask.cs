using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.SceneNavigation.Configuration;
using Modules.ResourceLoading.Contracts;
using Modules.SceneNavigation.Runtime.Catalogs;
using Modules.Startup.Contracts;
using UnityEngine.AddressableAssets;

namespace Game.SceneNavigation.Startup
{
    [Startup(StartupPhase.Foundation, Order = -1080)]
    public sealed class LoadSceneNavigationConfigStartupTask : IApplicationStartupTask, IDisposable
    {
        private readonly IResourceLoader _resourceLoader;
        private readonly AssetReference _configReference;
        private readonly SceneCatalog _catalog;

        private IResourceLease<SceneNavigationConfig> _configLease;

        public string Name => "Load scene navigation config";

        public LoadSceneNavigationConfigStartupTask(
            IResourceLoader resourceLoader,
            AssetReference configReference,
            SceneCatalog catalog)
        {
            _resourceLoader = resourceLoader ?? throw new ArgumentNullException(nameof(resourceLoader));
            _configReference = configReference ?? throw new ArgumentNullException(nameof(configReference));
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
        }

        public async UniTask ExecuteAsync(CancellationToken token)
        {
            if (_configLease != null)
                return;

            var configLease = await _resourceLoader.LoadAsync<SceneNavigationConfig>(
                _configReference,
                token);

            try
            {
                var config = configLease.Asset;
                config.Validate();

                _catalog.Initialize(config);

                _configLease = configLease;
                configLease = null;
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
    }
}