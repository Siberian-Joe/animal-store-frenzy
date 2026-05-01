using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Modules.ResourceLoading.Runtime.Contracts
{
    public interface IResourceLoader
    {
        UniTask<IResourceLease<TAsset>> LoadAsync<TAsset>(
            AssetReference reference,
            CancellationToken token)
            where TAsset : Object;

        UniTask<IResourceLease<TAsset>> LoadAsync<TAsset>(
            IResourceLocation location,
            CancellationToken token)
            where TAsset : Object;

        UniTask<IReadOnlyList<IResourceLocation>> LocateAsync<TAsset>(
            AssetLabelReference label,
            CancellationToken token)
            where TAsset : Object;
    }
}