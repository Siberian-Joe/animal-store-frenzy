using System.Collections.Generic;

namespace Game.World.Inventory
{
    public interface IInventory : IInventoryReader
    {
        void Add(ItemStack stack);
        bool TryRemove(ItemStack stack);
        bool TryRemove(IReadOnlyList<ItemStack> cost);
    }
}