using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.ResourceLoading.Contracts;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Game.ResourceLoading.Runtime
{
    public sealed class AddressablesResourceLoader : IResourceLoader
    {
        public async UniTask<IResourceLease<TAsset>> LoadAsync<TAsset>(
            AssetReference reference,
            CancellationToken token)
            where TAsset : Object
        {
            if (reference == null)
                throw new ArgumentNullException(nameof(reference));

            if (reference.RuntimeKeyIsValid() == false)
            {
                throw new InvalidOperationException(
                    $"{nameof(AssetReference)} does not contain a valid runtime key.");
            }

            AsyncOperationHandle<TAsset> handle = default;

            try
            {
                handle = Addressables.LoadAssetAsync<TAsset>(reference.RuntimeKey);

                var asset = await handle.ToUniTask(cancellationToken: token);

                if (asset == false)
                {
                    throw new InvalidOperationException(
                        $"Addressables returned null for runtime key '{reference.RuntimeKey}'.");
                }

                return new AddressablesResourceLease<TAsset>(asset, handle);
            }
            catch (OperationCanceledException)
            {
                Release(handle);
                throw;
            }
            catch
            {
                Release(handle);
                throw;
            }
        }

        private static void Release<TAsset>(AsyncOperationHandle<TAsset> handle)
            where TAsset : Object
        {
            if (handle.IsValid())
                Addressables.Release(handle);
        }

        private sealed class AddressablesResourceLease<TAsset> : IResourceLease<TAsset> where TAsset : Object
        {
            public TAsset Asset { get; }

            private AsyncOperationHandle<TAsset> _handle;
            private bool _disposed;

            public AddressablesResourceLease(
                TAsset asset,
                AsyncOperationHandle<TAsset> handle)
            {
                Asset = asset ?? throw new ArgumentNullException(nameof(asset));
                _handle = handle;
            }

            public void Dispose()
            {
                if (_disposed)
                    return;

                _disposed = true;

                Release(_handle);

                _handle = default;
            }
        }
    }
}