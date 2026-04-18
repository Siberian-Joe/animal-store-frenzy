using System.Collections.Generic;
using Game.World.EntityRuntime;

namespace Game.World.Persistence
{
    public interface ILiveEntityRegistry
    {
        IReadOnlyCollection<EntityRoot> All { get; }

        void Register(EntityRoot root);

        void Unregister(EntityRoot root);

        bool TryGet(EntityId id, out EntityRoot root);
    }
}
