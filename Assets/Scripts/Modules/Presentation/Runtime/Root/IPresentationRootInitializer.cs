using Modules.ResourceLoading.Contracts;
using UnityEngine;

namespace Modules.Presentation.Runtime.Root
{
    public interface IPresentationRootInitializer
    {
        bool IsInitialized { get; }

        void Initialize(
            PresentationRoot root,
            IResourceLease<GameObject> prefabLease = null);
    }
}