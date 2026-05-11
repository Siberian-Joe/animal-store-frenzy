using Game.World.Inventory;

namespace Game.World.Shop.Customers
{
    public interface ICustomerNeeds
    {
        bool HasActiveNeeds { get; }

        int ActiveNeedCount { get; }

        bool WantsItem(ItemId itemId);

        bool TrySatisfyItemNeed(ItemId itemId, float satisfactionAmount = 1f);
    }
}