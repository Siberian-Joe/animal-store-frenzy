using System;
using System.Collections.Generic;
using Game.World.Core;
using UnityEngine;
using Zenject;

namespace Game.World.Composition
{
    public sealed class SceneCompositionBootstrap : IInitializable, IDisposable
    {
        private readonly IEntityComposer _composer;
        private readonly IReadOnlyList<EntityRoot> _roots;

        public SceneCompositionBootstrap(
            IEntityComposer composer,
            IReadOnlyList<EntityRoot> roots)
        {
            _composer = composer ?? throw new ArgumentNullException(nameof(composer));
            _roots = roots ?? throw new ArgumentNullException(nameof(roots));
        }

        public void Initialize()
        {
            ValidateRoots();

            foreach (var root in _roots)
                _composer.Compose(root);
        }

        public void Dispose()
        {
        }

        private void ValidateRoots()
        {
            var rootsById = new Dictionary<string, EntityRoot>(StringComparer.Ordinal);

            foreach (var root in _roots)
            {
                if (root == false)
                {
                    throw new InvalidOperationException(
                        "Scene composition contains a null EntityRoot reference.");
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