using System.Collections.Generic;
using Game.World.EntityRuntime;
using Game.World.Shop;

namespace Game.World.UtilityAi
{
    public interface IShopUtilityOpportunityLocator
    {
        void CollectShelves(
            ProductId productId,
            EntityRoot excludedRoot,
            bool requireInteractionTarget,
            List<ShelfOpportunityEntry> results);

        void CollectCheckouts(
            EntityRoot excludedRoot,
            bool requireInteractionTarget,
            List<CheckoutOpportunityEntry> results);

        void CollectExits(
            EntityRoot excludedRoot,
            bool requireInteractionTarget,
            List<ExitOpportunityEntry> results);
    }
}