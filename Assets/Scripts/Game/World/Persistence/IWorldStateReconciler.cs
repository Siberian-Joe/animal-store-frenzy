using System.Collections.Generic;
using Game.World.EntityRuntime;

namespace Game.World.Persistence
{
    public interface IWorldStateReconciler
    {
        WorldStateReconcilePlan BuildPlan(
            IReadOnlyList<EntityRoot> persistentSceneRoots,
            IReadOnlyCollection<EntityState> persistedStates);
    }
}