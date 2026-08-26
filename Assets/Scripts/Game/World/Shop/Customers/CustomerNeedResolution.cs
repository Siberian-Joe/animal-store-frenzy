using System;
using Game.World.EntityRuntime;
using Game.World.Inventory;
using Game.World.Shop;

namespace Game.World.Shop.Customers
{
    public sealed class CustomerNeedResolution : ICustomerNeedResolution
    {
        private readonly IShopInteractionLocator _shopInteractionLocator;

        public CustomerNeedResolution(IShopInteractionLocator shopInteractionLocator) =>
            _shopInteractionLocator = shopInteractionLocator
                                      ?? throw new ArgumentNullException(nameof(shopInteractionLocator));

        public bool HasPendingShelfVisit(EntityRoot customerRoot, ICustomerNeeds customerNeeds)
        {
            ValidateArguments(customerRoot, customerNeeds);

            var needs = customerNeeds.Needs;
            if (needs == null || needs.Count == 0)
                return false;

            for (var index = 0; index < needs.Count; index++)
            {
                if (HasShelfTarget(customerRoot, needs[index]))
                    return true;
            }

            return false;
        }

        public int AbandonUnresolvableNeeds(EntityRoot customerRoot, ICustomerNeeds customerNeeds)
        {
            ValidateArguments(customerRoot, customerNeeds);

            var needs = customerNeeds.Needs;
            if (needs == null || needs.Count == 0)
                return 0;

            var abandonedCount = 0;

            for (var index = needs.Count - 1; index >= 0; index--)
            {
                var need = needs[index];
                if (IsNeedActive(need) == false)
                    continue;

                if (HasShelfTarget(customerRoot, need))
                    continue;

                if (customerNeeds.TryAbandonNeed(need.NeedId))
                    abandonedCount++;
            }

            return abandonedCount;
        }

        private bool HasShelfTarget(EntityRoot customerRoot, CustomerNeedState need)
        {
            if (IsNeedActive(need) == false)
                return false;

            return _shopInteractionLocator.TryFindShelf(
                new ItemId(need.ItemId),
                customerRoot.transform.position,
                customerRoot,
                requireInteractionTarget: true,
                out _);
        }

        private static bool IsNeedActive(CustomerNeedState need) =>
            need != null &&
            string.IsNullOrWhiteSpace(need.NeedId) == false &&
            string.IsNullOrWhiteSpace(need.ItemId) == false &&
            need.Intensity > 0f;

        private static void ValidateArguments(EntityRoot customerRoot, ICustomerNeeds customerNeeds)
        {
            if (customerRoot == false)
                throw new ArgumentNullException(nameof(customerRoot));

            if (customerNeeds == null)
                throw new ArgumentNullException(nameof(customerNeeds));
        }
    }
}
