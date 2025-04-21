using Cysharp.Threading.Tasks;
using UnityEngine;

namespace NewCore.Services.ResourceLoaders
{
    public interface IResourceLoader
    {
        UniTask<TResource> LoadResourceAsync<TResource>() where TResource : class;
        UniTask<TResource> InstantiateResourceAsync<TResource>(Transform parent = null) where TResource : class;
    }
}