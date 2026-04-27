using System;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Game.Presentation.Contracts.Catalog
{
    public interface IPanelCatalog
    {
        IResourceLocation GetPrefabLocation(Type panelType);
    }
}