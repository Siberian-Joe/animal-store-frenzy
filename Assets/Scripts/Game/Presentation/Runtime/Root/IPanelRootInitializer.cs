using Game.ResourceLoading.Contracts;
using UnityEngine;

namespace Game.Presentation.Runtime.Root
{
    public interface IPanelRootInitializer
    {
        void Initialize(PanelRoot root, IResourceLease<GameObject> prefabLease);
    }
}