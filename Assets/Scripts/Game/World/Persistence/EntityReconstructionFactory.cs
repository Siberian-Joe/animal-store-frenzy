using System;
using Game.World.EntityRuntime;
using UnityEngine;

namespace Game.World.Persistence
{
    public sealed class EntityReconstructionFactory : IEntityReconstructionFactory
    {
        private readonly IEntityFactory _entityFactory;
        private readonly IEntityBlueprintCatalog _blueprints;

        public EntityReconstructionFactory(
            IEntityFactory entityFactory,
            IEntityBlueprintCatalog blueprints)
        {
            _entityFactory = entityFactory ?? throw new ArgumentNullException(nameof(entityFactory));
            _blueprints = blueprints ?? throw new ArgumentNullException(nameof(blueprints));
        }

        public EntityRoot Reconstruct(EntityState state)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            if (string.IsNullOrWhiteSpace(state.BlueprintId))
            {
                throw new InvalidOperationException(
                    $"Cannot reconstruct entity '{state.Id}' because it has no BlueprintId.");
            }

            if (_blueprints.TryGetPrefab(state.BlueprintId, out var prefab) == false || prefab == false)
            {
                throw new InvalidOperationException(
                    $"No entity blueprint prefab is registered for BlueprintId '{state.BlueprintId}'.");
            }

            return _entityFactory.CreateFromPrefab(
                prefab,
                state.Id,
                Vector3.zero,
                Quaternion.identity);
        }

        public void Destroy(EntityRoot root)
        {
            _entityFactory.Destroy(root);
        }
    }
}
