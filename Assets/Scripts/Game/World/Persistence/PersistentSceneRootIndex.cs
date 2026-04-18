using System;
using System.Collections.Generic;
using Game.World.EntityRuntime;

namespace Game.World.Persistence
{
    public sealed class PersistentSceneRootIndex : IPersistentSceneRootIndex
    {
        public IReadOnlyDictionary<EntityId, EntityRoot> Build(IReadOnlyList<EntityRoot> roots)
        {
            if (roots == null)
                throw new ArgumentNullException(nameof(roots));

            var result = new Dictionary<EntityId, EntityRoot>();

            for (var index = 0; index < roots.Count; index++)
            {
                var root = roots[index];
                var id = root.Id;

                if (result.TryGetValue(id, out var existingRoot))
                {
                    throw new InvalidOperationException(
                        $"Duplicate persistent scene EntityId '{id}' detected on '{existingRoot.name}' and '{root.name}'.");
                }

                result.Add(id, root);
            }

            return result;
        }
    }
}