using System;
using Object = UnityEngine.Object;

namespace Game.ResourceLoading.Contracts
{
    public interface IResourceLease<out TAsset> : IDisposable where TAsset : Object
    {
        TAsset Asset { get; }
    }
}