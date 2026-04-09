using System;
using System.Collections.Generic;

namespace Game.World.EntityRuntime
{
    public interface IEntityStateStore
    {
        IReadOnlyCollection<EntityState> All { get; }

        bool TryGet(EntityId id, out EntityState state);

        EntityState GetOrCreate(EntityId id, Func<EntityState> factory);

        void Save(EntityState state);
    }
}