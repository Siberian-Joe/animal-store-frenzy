using System;
using Object = UnityEngine.Object;

namespace Modules.ResourceLoading.Contracts
{
    public interface IResourceLease<out TAsset> : IDisposable
        where TAsset : Object
    {
        TAsset Asset { get; }
    }
}