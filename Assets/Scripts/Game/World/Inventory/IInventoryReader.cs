using System.Collections.Generic;
using R3;

namespace Game.World.Inventory
{
    public interface IInventoryReader
    {
        IReadOnlyList<ItemStack> Items { get; }
        Observable<Unit> Changed { get; }
        int GetAmount(ItemId itemId);
        bool Has(ItemStack stack);
        bool Has(IReadOnlyList<ItemStack> cost);
    }
}