using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Modules.ResourceLoading.Runtime.Contracts;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using Object = UnityEngine.Object;

namespace Modules.ResourceLoading.Runtime
{
    public sealed class ResourceLoader : IResourceLoader
    {
        public UniTask<IResourceLease<TAsset>> LoadAsync<TAsset>(
            AssetReference reference,
            CancellationToken token)
            where TAsset : Object
        {
            if (reference == null)
                throw new ArgumentNullException(nameof(reference));

            return reference.RuntimeKeyIsValid() == false
                ? throw new InvalidOperationException($"{nameof(AssetReference)} does not contain a valid runtime key.")
                : LoadInternalAsync(() => Addressables.LoadAssetAsync<TAsset>(reference.RuntimeKey), token);
        }

        public UniTask<IResourceLease<TAsset>> LoadAsync<TAsset>(
            IResourceLocation location,
            CancellationToken token)
            where TAsset : Object =>
            location == null
                ? throw new ArgumentNullException(nameof(location))
                : LoadInternalAsync(() => Addressables.LoadAssetAsync<TAsset>(location), token);

        public async UniTask<IReadOnlyList<IResourceLocation>> LocateAsync<TAsset>(
            AssetLabelReference label,
            CancellationToken token)
            where TAsset : Object
        {
            if (label == null)
                throw new ArgumentNullException(nameof(label));

            if (string.IsNullOrWhiteSpace(label.labelString))
                throw new InvalidOperationException($"{nameof(AssetLabelReference)} contains empty label.");

            AsyncOperationHandle<IList<IResourceLocation>> handle = default;

            try
            {
                handle = Addressables.LoadResourceLocationsAsync(
                    label.RuntimeKey,
                    typeof(TAsset));

                var locations = await handle.ToUniTask(cancellationToken: token);

                return locations == null
                    ? Array.Empty<IResourceLocation>()
                    : new List<IResourceLocation>(locations);
            }
            finally
            {
                if (handle.IsValid())
                    Addressables.Release(handle);
            }
        }

        private static async UniTask<IResourceLease<TAsset>> LoadInternalAsync<TAsset>(
            Func<AsyncOperationHandle<TAsset>> load,
            CancellationToken token)
            where TAsset : Object
        {
            AsyncOperationHandle<TAsset> handle = default;

            try
            {
                handle = load();

                var asset = await handle.ToUniTask(cancellationToken: token);

                if (asset == false)
                    throw new InvalidOperationException(
                        $"Addressables returned null asset for '{typeof(TAsset).FullName}'.");

                return new ResourceLease<TAsset>(asset, handle);
            }
            catch
            {
                if (handle.IsValid())
                    Addressables.Release(handle);

                throw;
            }
        }

        private sealed class ResourceLease<TAsset> : IResourceLease<TAsset>
            where TAsset : Object
        {
            public TAsset Asset { get; }

            private AsyncOperationHandle<TAsset> _handle;
            private bool _disposed;

            public ResourceLease(TAsset asset, AsyncOperationHandle<TAsset> handle)
            {
                Asset = asset ? asset : throw new ArgumentNullException(nameof(asset));
                _handle = handle;
            }

            public void Dispose()
            {
                if (_disposed)
                    return;

                _disposed = true;

                if (_handle.IsValid())
                    Addressables.Release(_handle);

                _handle = default;
            }
        }
    }
}