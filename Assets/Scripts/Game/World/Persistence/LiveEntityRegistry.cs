using System;
using System.Collections.Generic;
using Game.World.EntityRuntime;
using UnityEngine;
using EntityId = Game.World.EntityRuntime.EntityId;

namespace Game.World.Persistence
{
    public sealed class LiveEntityRegistry : ILiveEntityRegistry
    {
        private readonly Dictionary<EntityId, EntityRoot> _rootsById = new();
        private readonly Dictionary<EntityRoot, EntityId> _idsByRoot = new();

        public IReadOnlyCollection<EntityRoot> All => _rootsById.Values;

        public void Register(EntityRoot root)
        {
            if (root == false)
                throw new ArgumentNullException(nameof(root));

            var identifier = root.Identifier;
            var rawId = identifier ? identifier.Id : null;

            if (string.IsNullOrWhiteSpace(rawId))
            {
                throw new InvalidOperationException(
                    $"EntityRoot '{GetHierarchyPath(root.transform)}' has an empty EntityId.");
            }

            var id = new EntityId(rawId);

            if (_idsByRoot.ContainsKey(root))
            {
                throw new InvalidOperationException(
                    $"EntityRoot '{GetHierarchyPath(root.transform)}' is already registered as live.");
            }

            if (_rootsById.TryGetValue(id, out var existingRoot))
            {
                throw new InvalidOperationException(
                    $"Duplicate live EntityId '{id}' detected for roots " +
                    $"'{GetHierarchyPath(existingRoot.transform)}' and '{GetHierarchyPath(root.transform)}'.");
            }

            _rootsById.Add(id, root);
            _idsByRoot.Add(root, id);
        }

        public void Unregister(EntityRoot root)
        {
            if (root == false)
                return;

            if (_idsByRoot.Remove(root, out var id) == false)
                return;

            if (_rootsById.TryGetValue(id, out var registeredRoot) && registeredRoot == root)
                _rootsById.Remove(id);
        }

        public bool TryGet(EntityId id, out EntityRoot root) =>
            _rootsById.TryGetValue(id, out root);

        private static string GetHierarchyPath(Transform transform)
        {
            if (transform == false)
                return "<null>";

            var path = transform.name;
            var current = transform.parent;

            while (current)
            {
                path = $"{current.name}/{path}";
                current = current.parent;
            }

            return path;
        }
    }
}