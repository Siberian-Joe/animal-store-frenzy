using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Modules.SceneNavigation.Runtime.Contracts;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Modules.SceneNavigation.Runtime.Loaders
{
    public sealed class SceneLoader : ISceneLoader
    {
        public async UniTask<Scene> LoadSingleAsync(AssetReference reference, CancellationToken token)
        {
            if (reference == null)
                throw new ArgumentNullException(nameof(reference));

            if (reference.RuntimeKeyIsValid() == false)
                throw new InvalidOperationException(
                    $"{nameof(AssetReference)} contains invalid runtime key.");

            AsyncOperationHandle<SceneInstance> handle = default;

            try
            {
                handle = Addressables.LoadSceneAsync(
                    reference.RuntimeKey);

                var sceneInstance = await handle.ToUniTask(cancellationToken: token);

                return sceneInstance.Scene;
            }
            catch
            {
                if (handle.IsValid())
                    Addressables.Release(handle);

                throw;
            }
        }
    }
}