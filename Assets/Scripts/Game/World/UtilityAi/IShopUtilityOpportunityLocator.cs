using System.Collections.Generic;
using Game.World.EntityRuntime;
using Game.World.Inventory;

namespace Game.World.UtilityAi
{
    public interface IShopUtilityOpportunityLocator
    {
        void CollectShelves(
            ItemId itemId,
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