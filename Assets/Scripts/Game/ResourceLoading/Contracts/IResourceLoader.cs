using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.ResourceLoading.Contracts
{
    public interface IResourceLoader
    {
        UniTask<IResourceLease<TAsset>> LoadAsync<TAsset>(
            AssetReference reference,
            CancellationToken token)
            where TAsset : Object;
    }
}