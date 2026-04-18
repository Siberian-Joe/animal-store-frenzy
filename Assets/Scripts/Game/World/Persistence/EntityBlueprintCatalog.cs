using System;
using System.Collections.Generic;
using Game.World.EntityRuntime;

namespace Game.World.Persistence
{
    public sealed class EntityBlueprintCatalog : IEntityBlueprintCatalog
    {
        private readonly Dictionary<string, EntityRoot> _prefabsById;

        public EntityBlueprintCatalog(Dictionary<string, EntityRoot> prefabsById) =>
            _prefabsById = prefabsById ?? throw new ArgumentNullException(nameof(prefabsById));

        public bool TryGetPrefab(string blueprintId, out EntityRoot prefab)
        {
            if (string.IsNullOrWhiteSpace(blueprintId) == false)
                return _prefabsById.TryGetValue(blueprintId.Trim(), out prefab) && prefab != false;

            prefab = null;
            return false;
        }
    }
}