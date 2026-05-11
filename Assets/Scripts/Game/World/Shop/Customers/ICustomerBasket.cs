using Game.World.Inventory;

namespace Game.World.Shop.Customers
{
    public interface ICustomerBasket
    {
        bool HasItems { get; }

        int TotalItemCount { get; }

        int UniqueItemCount { get; }

        int GetQuantity(ItemId itemId);

        void AddItem(ItemId itemId, int quantity);

        void ClearBasket();
    }
}