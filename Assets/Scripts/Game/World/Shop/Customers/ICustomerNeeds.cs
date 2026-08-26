using System.Collections.Generic;
using Game.World.Inventory;

namespace Game.World.Shop.Customers
{
    public interface ICustomerNeeds
    {
        bool HasActiveNeeds { get; }

        int ActiveNeedCount { get; }

        IReadOnlyList<CustomerNeedState> Needs { get; }

        bool WantsItem(ItemId itemId);

        bool TryAbandonNeed(string needId);

        bool TrySatisfyItemNeed(ItemId itemId, float satisfactionAmount = 1f);
    }
}