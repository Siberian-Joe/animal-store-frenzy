using System.Collections.Generic;
using Game.World.EntityRuntime;

namespace Game.World.Persistence
{
    public interface IPersistentSceneRootCatalog
    {
        IReadOnlyList<EntityRoot> Collect(IReadOnlyList<EntityRoot> sceneRoots);
    }
}