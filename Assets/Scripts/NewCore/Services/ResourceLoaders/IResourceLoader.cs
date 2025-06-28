using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace NewCore.Services.ResourceLoaders
{
    public interface IResourceLoader
    {
        UniTask<TResource> LoadResourceAsync<TResource>(CancellationToken cancellationToken = default) where TResource : Object;
        UniTask<TResource> InstantiateResourceAsync<TResource>(Transform parent = null, CancellationToken cancellationToken = default) where TResource : Object;
    }
}