using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace NewCore.Services.Addressables
{
    public class AddressableResourceLoader : IAddressableResourceLoader
    {
        public async UniTask<TResource> LoadResourceAsync<TResource>() where TResource : class =>
            await ExecuteAsync<TResource>(UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<GameObject>);

        public async UniTask<TResource> InstantiateResourceAsync<TResource>(Transform parent = null)
            where TResource : class =>
            await ExecuteAsync<TResource>(key => UnityEngine.AddressableAssets.Addressables.InstantiateAsync(key, parent, false, false));

        private static async UniTask<TResource> ExecuteAsync<TResource>(
            Func<string, AsyncOperationHandle<GameObject>> operation)
            where TResource : class
        {
            var key = typeof(TResource).Name;
            try
            {
                var handle = operation(key);
                await handle.ToUniTask();

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
            catch (Exception ex)
            {
                Debug.LogError($"Exception while executing operation for resource with key {key}: {ex}");
                return null;
            }
        }
    }
}