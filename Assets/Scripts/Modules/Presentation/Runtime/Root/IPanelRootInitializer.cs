using Modules.ResourceLoading.Contracts;
using UnityEngine;

namespace Modules.Presentation.Runtime.Root
{
    public interface IPanelRootInitializer
    {
        void Initialize(PanelRoot root, IResourceLease<GameObject> prefabLease);
    }
}