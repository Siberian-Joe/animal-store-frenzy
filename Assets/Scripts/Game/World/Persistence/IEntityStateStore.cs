using System.Collections.Generic;
using Game.World.EntityRuntime;

namespace Game.World.Persistence
{
    public interface IEntityStateStore
    {
        bool HasSnapshot { get; }

        IReadOnlyCollection<EntityState> All { get; }

        bool TryGet(EntityId id, out EntityState state);

        void Save(EntityState state);

        bool Remove(EntityId id);
    }
}
