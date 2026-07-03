using Game.World.Commands;
using Game.World.EntityRuntime;

namespace Game.World.Store
{
    public sealed class CloseStoreCommand : IGameCommand
    {
        public CloseStoreCommand(EntityId storeId) => StoreId = storeId;

        public EntityId StoreId { get; }
    }
}