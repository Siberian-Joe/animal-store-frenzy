using System.Collections.Generic;
using Game.World.EntityRuntime;

namespace Game.World.Persistence
{
    public sealed class WorldStateReconcilePlan
    {
        public List<EntityRoot> SceneRootsToActivate { get; } = new();
        public List<EntityState> StatesToReconstruct { get; } = new();
        public List<EntityRoot> SceneRootsToRemove { get; } = new();
    }
}