using UnityEngine;

namespace Game.World.EntityRuntime
{
    public interface IEntityFactory
    {
        EntityRoot Create(
            string entityName,
            EntityId entityId,
            Vector3 position,
            Quaternion rotation);

        EntityRoot CreateFromPrefab(
            EntityRoot prefab,
            EntityId entityId,
            Vector3 position,
            Quaternion rotation);

        void Destroy(EntityRoot root);
    }
}