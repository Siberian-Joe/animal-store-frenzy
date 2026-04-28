using System;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Game.World.EntityRuntime
{
    public sealed class RuntimeEntityFactory : IEntityFactory
    {
        private readonly DiContainer _container;

        public RuntimeEntityFactory(DiContainer container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public EntityRoot Create(
            string entityName,
            EntityId entityId,
            Vector3 position,
            Quaternion rotation)
        {
            if (string.IsNullOrWhiteSpace(entityName))
                throw new ArgumentException("Entity name cannot be null or whitespace.", nameof(entityName));

            var gameObject = new GameObject(entityName);
            gameObject.transform.SetPositionAndRotation(position, rotation);

            var identifier = gameObject.AddComponent<EntityIdentifier>();
            identifier.Id = entityId.Value;

            return gameObject.AddComponent<EntityRoot>();
        }

        public EntityRoot CreateFromPrefab(
            EntityRoot prefab,
            EntityId entityId,
            Vector3 position,
            Quaternion rotation)
        {
            if (prefab == false)
                throw new ArgumentNullException(nameof(prefab));

            var instance = _container.InstantiatePrefabForComponent<EntityRoot>(
                prefab.gameObject,
                position,
                rotation,
                null);

            if (instance == false)
            {
                throw new InvalidOperationException(
                    $"Infrastructure failed to instantiate entity prefab '{prefab.name}'.");
            }

            instance.Identifier.Id = entityId.Value;
            return instance;
        }

        public void Destroy(EntityRoot root)
        {
            if (root == false)
                return;

            Object.Destroy(root.gameObject);
        }
    }
}