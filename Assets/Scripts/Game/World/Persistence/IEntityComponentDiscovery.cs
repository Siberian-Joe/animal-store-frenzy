using System.Collections.Generic;
using Game.World.EntityRuntime;

namespace Game.World.Persistence
{
    public interface IEntityComponentDiscovery
    {
        IReadOnlyList<IEntityComponent> Discover(EntityRoot root);
    }
}