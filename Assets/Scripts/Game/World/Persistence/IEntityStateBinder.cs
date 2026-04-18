using System.Collections.Generic;
using Game.World.EntityRuntime;

namespace Game.World.Persistence
{
    public interface IEntityStateBinder
    {
        void Bind(IReadOnlyList<IEntityComponent> components, EntityState workingState);
    }
}