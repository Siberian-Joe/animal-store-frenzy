using System.Collections.Generic;
using Game.World.EntityRuntime;

namespace Game.World.Persistence
{
    public interface IPersistentSceneRootIndex
    {
        IReadOnlyDictionary<EntityId, EntityRoot> Build(IReadOnlyList<EntityRoot> roots);
    }
}