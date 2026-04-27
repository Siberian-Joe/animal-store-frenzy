using System;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Game.Presentation.Runtime.Catalog
{
    public interface IPanelCatalogInitializer
    {
        bool IsInitialized { get; }

        void Initialize(IReadOnlyDictionary<Type, IResourceLocation> prefabLocationsByPanelType);
    }
}