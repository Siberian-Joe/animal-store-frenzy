using System;
using System.Collections.Generic;
using Modules.Presentation.Runtime.Contracts.Catalog;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Modules.Presentation.Runtime.Catalog
{
    public sealed class PanelCatalog : IPanelCatalog, IPanelCatalogInitializer
    {
        private IReadOnlyDictionary<Type, IResourceLocation> _prefabLocationsByPanelType;

        public bool IsInitialized => _prefabLocationsByPanelType != null;

        public void Initialize(IReadOnlyDictionary<Type, IResourceLocation> prefabLocationsByPanelType)
        {
            if (_prefabLocationsByPanelType != null)
                throw new InvalidOperationException($"{nameof(PanelCatalog)} is already initialized.");

            _prefabLocationsByPanelType = prefabLocationsByPanelType
                                          ?? throw new ArgumentNullException(nameof(prefabLocationsByPanelType));
        }

        public IResourceLocation GetPrefabLocation(Type panelType)
        {
            if (panelType == null)
                throw new ArgumentNullException(nameof(panelType));

            if (_prefabLocationsByPanelType == null)
                throw new InvalidOperationException($"{nameof(PanelCatalog)} was used before initialization.");

            return _prefabLocationsByPanelType.TryGetValue(panelType, out var location)
                ? location
                : throw new InvalidOperationException(
                    $"Panel catalog does not contain panel '{panelType.FullName}'.");
        }
    }
}