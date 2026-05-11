using Game.World.Interactions;
using Game.World.Inventory;

namespace Game.World.Shop.Customers
{
    public interface ICustomerShoppingRole : IInteractionRole
    {
        bool WantsItem(ItemId itemId);
    }
}