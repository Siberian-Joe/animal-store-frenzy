using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using NewCore.Attributes;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace NewCore.Services.ResourceLoaders
{
    public class ResourceLoader : IResourceLoader
    {
        private readonly ConcurrentDictionary<Type, string> _keyCache = new();

        public async UniTask<TResource> LoadResourceAsync<TResource>(CancellationToken cancellationToken)
            where TResource : class =>
            await ExecuteAsync<TResource>(Addressables.LoadAssetAsync<GameObject>, cancellationToken);

        public async UniTask<TResource> InstantiateResourceAsync<TResource>(Transform parent = null,
            CancellationToken cancellationToken = default)
            where TResource : class =>
            await ExecuteAsync<TResource>(key => Addressables.InstantiateAsync(key, parent, false, false),
                cancellationToken);

        private async UniTask<TResource> ExecuteAsync<TResource>(
            Func<string, AsyncOperationHandle<GameObject>> operation,
            CancellationToken cancellationToken = default)
            where TResource : class
        {
            var key = _keyCache.GetOrAdd(typeof(TResource), type =>
            {
                var attribute = type.GetCustomAttribute<ResourceKeyAttribute>(false);
                return attribute?.Key ?? type.Name;
            });

            try
            {
                var handle = operation(key);
                await handle.ToUniTask(cancellationToken: cancellationToken);

                if (handle.Status != AsyncOperationStatus.Succeeded)
                {
                    Debug.LogError($"Failed to execute operation for resource. Key: {key}");
                    return null;
                }

                var component = handle.Result.GetComponent<TResource>();
                if (component == null)
                {
                    Debug.LogError($"Component of type {typeof(TResource)} not found on GameObject with key {key}");
                    return null;
                }

                return component;
            }
            catch (Exception exception)
            {
                Debug.LogError($"Exception while executing operation for resource with key {key}: {exception}");
                return null;
            }
        }
    }
}