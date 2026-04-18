using System;
using System.Collections.Generic;
using Game.World.EntityRuntime;

namespace Game.World.Persistence
{
    public sealed class WorldStateReconciler : IWorldStateReconciler
    {
        private readonly IPersistentSceneRootIndex _index;

        public WorldStateReconciler(IPersistentSceneRootIndex index) =>
            _index = index ?? throw new ArgumentNullException(nameof(index));

        public WorldStateReconcilePlan BuildPlan(
            IReadOnlyList<EntityRoot> persistentSceneRoots,
            IReadOnlyCollection<EntityState> persistedStates)
        {
            if (persistentSceneRoots == null)
                throw new ArgumentNullException(nameof(persistentSceneRoots));
            if (persistedStates == null)
                throw new ArgumentNullException(nameof(persistedStates));

            var plan = new WorldStateReconcilePlan();
            var sceneRootsById = _index.Build(persistentSceneRoots);
            var matchedIds = new HashSet<EntityId>();

            foreach (var state in persistedStates)
            {
                if (state == null)
                    continue;

                matchedIds.Add(state.Id);

                if (sceneRootsById.TryGetValue(state.Id, out var sceneRoot))
                {
                    plan.SceneRootsToActivate.Add(sceneRoot);
                    continue;
                }

                plan.StatesToReconstruct.Add(state);
            }

            foreach (var sceneRoot in persistentSceneRoots)
            {
                if (matchedIds.Contains(sceneRoot.Id))
                    continue;

                plan.SceneRootsToRemove.Add(sceneRoot);
            }

            return plan;
        }
    }
}