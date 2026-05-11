using Game.World.EntityRuntime;
using Game.World.Inventory;
using UnityEngine;

namespace Game.World.Shop
{
    public interface IShopInteractionLocator
    {
        bool TryFindShelf(
            ItemId itemId,
            Vector3 origin,
            EntityRoot excludedRoot,
            bool requireInteractionTarget,
            out EntityRoot targetRoot);

        bool TryFindCheckout(
            Vector3 origin,
            EntityRoot excludedRoot,
            bool requireInteractionTarget,
            out EntityRoot targetRoot);

        bool TryFindExit(
            Vector3 origin,
            EntityRoot excludedRoot,
            bool requireInteractionTarget,
            out EntityRoot targetRoot);
    }
}