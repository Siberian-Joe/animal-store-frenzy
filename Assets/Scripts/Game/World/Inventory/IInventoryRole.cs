using System.Collections.Generic;
using Game.World.Interactions;

namespace Game.World.Inventory
{
    public interface IInventoryRole : IInteractionRole
    {
        IInventory Inventory { get; }
        bool CanAccept(ItemStack stack);
        bool CanPay(IReadOnlyList<ItemStack> cost);
    }
}