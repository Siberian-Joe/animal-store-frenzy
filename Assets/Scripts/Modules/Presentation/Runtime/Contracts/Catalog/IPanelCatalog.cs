using System;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Modules.Presentation.Runtime.Contracts.Catalog
{
    public interface IPanelCatalog
    {
        IResourceLocation GetPrefabLocation(Type panelType);
    }
}