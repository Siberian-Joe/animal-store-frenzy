using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.ResourceLoading.Contracts;
using Game.Startup.Contracts;
using UnityEngine.AddressableAssets;

namespace Game.World.Shop.Products
{
    [Startup(StartupPhase.Preparation)]
    public sealed class ShopProductCatalogStartupTask : IStartupTask, IDisposable
    {
        public string Name => "Shop product catalog preload";

        private readonly IResourceLoader _resourceLoader;
        private readonly IShopProductCatalogConfig _config;
        private readonly IShopProductCatalogInitializer _initializer;

        private readonly List<IResourceLease<ProductDefinition>> _leases = new(16);

        private bool _disposed;
        private bool _executed;

        public ShopProductCatalogStartupTask(
            IResourceLoader resourceLoader,
            IShopProductCatalogConfig config,
            IShopProductCatalogInitializer bootstrap)
        {
            _resourceLoader = resourceLoader ?? throw new ArgumentNullException(nameof(resourceLoader));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _initializer = bootstrap ?? throw new ArgumentNullException(nameof(bootstrap));
        }

        public async UniTask ExecuteAsync(CancellationToken token)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(ShopProductCatalogStartupTask));

            if (_executed)
                throw new InvalidOperationException(
                    $"{nameof(ShopProductCatalogStartupTask)} cannot be executed more than once.");

            var references = _config.ProductReferences
                             ?? throw new InvalidOperationException(
                                 $"{nameof(IShopProductCatalogConfig)} returned null references.");

            var products = new List<ProductDefinition>(references.Count);

            for (var index = 0; index < references.Count; index++)
            {
                token.ThrowIfCancellationRequested();

                var reference = references[index];
                if (reference == null)
                {
                    throw new InvalidOperationException(
                        $"{nameof(ShopProductCatalogStartupTask)} contains a null {nameof(AssetReference)} at index {index}.");
                }

                var lease = await _resourceLoader.LoadAsync<ProductDefinition>(reference, token);
                _leases.Add(lease);
                products.Add(lease.Asset);
            }

            _initializer.Initialize(products);
            _executed = true;
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            for (var index = _leases.Count - 1; index >= 0; index--)
                _leases[index]?.Dispose();

            _leases.Clear();
        }
    }
}