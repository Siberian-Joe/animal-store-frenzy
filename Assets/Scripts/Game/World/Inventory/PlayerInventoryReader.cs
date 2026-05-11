using System;
using Game.World.EntityRuntime;
using R3;

namespace Game.World.Inventory
{
    public sealed class PlayerInventoryReader : IPlayerInventoryReader
    {
        private readonly EntityRoot _playerRoot;
        private IInventoryReader _inventory;

        public PlayerInventoryReader(EntityRoot playerRoot) => _playerRoot =
            playerRoot ? playerRoot : throw new ArgumentNullException(nameof(playerRoot));

        public IInventoryReader Inventory => _inventory ??= ResolveInventory();
        public Observable<Unit> Changed => Inventory.Changed;

        private IInventoryReader ResolveInventory()
        {
            var inventory = _playerRoot.FindOwnedComponent<IInventoryReader>();
            if (inventory == null)
                throw new InvalidOperationException(
                    $"Player entity '{_playerRoot.name}' has no {nameof(IInventoryReader)} component.");

            return inventory;
        }
    }
}