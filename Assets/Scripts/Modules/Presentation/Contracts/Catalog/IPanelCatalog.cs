using System;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Modules.Presentation.Contracts.Catalog
{
    public interface IPanelCatalog
    {
        IResourceLocation GetPrefabLocation(Type panelType);
    }
}