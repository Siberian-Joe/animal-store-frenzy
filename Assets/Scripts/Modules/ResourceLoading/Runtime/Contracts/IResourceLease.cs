using System;
using Object = UnityEngine.Object;

namespace Modules.ResourceLoading.Runtime.Contracts
{
    public interface IResourceLease<out TAsset> : IDisposable
        where TAsset : Object
    {
        TAsset Asset { get; }
    }
}