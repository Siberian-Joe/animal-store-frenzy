using System;
using Game.World.Commands;
using Game.World.EntityRuntime;

namespace Game.World.Store
{
    [Serializable]
    public sealed class OpenStoreCommand : IGameCommand
    {
        public OpenStoreCommand(EntityId storeId) => StoreId = storeId;

        public EntityId StoreId { get; }
    }
}
