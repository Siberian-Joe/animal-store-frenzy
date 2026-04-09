using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.World.EntityRuntime
{
    public sealed class SceneEntityBootstrap : IInitializable, IDisposable
    {
        private readonly EntityActivator _activator;
        private readonly IReadOnlyList<EntityRoot> _roots;

        public SceneEntityBootstrap(
            EntityActivator activator,
            IReadOnlyList<EntityRoot> roots)
        {
            _activator = activator ?? throw new ArgumentNullException(nameof(activator));
            _roots = roots ?? throw new ArgumentNullException(nameof(roots));
        }

        public void Initialize()
        {
            ValidateRoots();

            var activatedRoots = new List<EntityRoot>(_roots.Count);

            try
            {
                foreach (var root in _roots)
                {
                    _activator.Activate(root);
                    activatedRoots.Add(root);
                }
            }
            catch
            {
                for (var index = activatedRoots.Count - 1; index >= 0; index--)
                    _activator.Deactivate(activatedRoots[index]);

                throw;
            }
        }

        public void Dispose()
        {
            for (var index = _roots.Count - 1; index >= 0; index--)
            {
                var root = _roots[index];
                if (root == false)
                    continue;

                _activator.Deactivate(root);
            }
        }

        private void ValidateRoots()
        {
            var rootsById = new Dictionary<string, EntityRoot>(StringComparer.Ordinal);

            foreach (var root in _roots)
            {
                if (root == false)
                {
                    throw new InvalidOperationException(
                        "Scene entity bootstrap contains a null EntityRoot reference.");
                }

                var identifier = root.Identifier;
                var rawId = identifier ? identifier.Id : null;

                if (string.IsNullOrWhiteSpace(rawId))
                {
                    throw new InvalidOperationException(
                        $"EntityRoot '{GetHierarchyPath(root.transform)}' has an empty EntityId.");
                }

                if (rootsById.TryGetValue(rawId, out var existingRoot))
                {
                    throw new InvalidOperationException(
                        $"Duplicate EntityId '{rawId}' detected for roots " +
                        $"'{GetHierarchyPath(existingRoot.transform)}' and '{GetHierarchyPath(root.transform)}'.");
                }

                rootsById.Add(rawId, root);
            }
        }

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
