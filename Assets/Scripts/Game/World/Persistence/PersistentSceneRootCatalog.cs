using System;
using System.Collections.Generic;
using Game.World.EntityRuntime;

namespace Game.World.Persistence
{
    public sealed class PersistentSceneRootCatalog : IPersistentSceneRootCatalog
    {
        public IReadOnlyList<EntityRoot> Collect(IReadOnlyList<EntityRoot> sceneRoots)
        {
            if (sceneRoots == null)
                throw new ArgumentNullException(nameof(sceneRoots));

            var persistentRoots = new List<EntityRoot>();
            var seenRoots = new HashSet<EntityRoot>();

            foreach (var root in sceneRoots)
            {
                if (root == false)
                    throw new InvalidOperationException(
                        "World persistence bootstrap contains a null EntityRoot reference.");

                if (seenRoots.Add(root) == false)
                    continue;

                if (IsPersistent(root) == false)
                    continue;

                persistentRoots.Add(root);
            }

            return persistentRoots;
        }

        private static bool IsPersistent(EntityRoot root)
        {
            var components = root.GetComponentsInChildren<EntityComponent>(true);
            foreach (var component in components)
            {
                if (component == false || component.OwnerRoot != root)
                    continue;

                if (component is IEntityStateBindingBridge)
                    return true;
            }

            return false;
        }
    }
}