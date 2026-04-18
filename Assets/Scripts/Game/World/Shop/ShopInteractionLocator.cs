using System;
using System.Collections.Generic;
using Game.World.EntityRuntime;
using Game.World.Features.InteractionTarget;
using Game.World.Shop.Checkouts;
using Game.World.Shop.Exits;
using Game.World.Shop.Shelves;
using UnityEngine;

namespace Game.World.Shop
{
    public sealed class ShopInteractionLocator :
        IShopInteractionLocator,
        IShopInteractionRegistry
    {
        private readonly List<ShelfProductPart> _shelves = new(8);
        private readonly List<CheckoutCounterPart> _checkouts = new(4);
        private readonly List<StoreExitPointPart> _exits = new(4);

        public void Register(ShelfProductPart shelf) => Register(_shelves, shelf);

        public void Unregister(ShelfProductPart shelf) => Unregister(_shelves, shelf);

        public void Register(CheckoutCounterPart checkout) => Register(_checkouts, checkout);

        public void Unregister(CheckoutCounterPart checkout) => Unregister(_checkouts, checkout);

        public void Register(StoreExitPointPart exitPoint) => Register(_exits, exitPoint);

        public void Unregister(StoreExitPointPart exitPoint) => Unregister(_exits, exitPoint);

        public bool TryFindShelf(
            ProductId productId,
            Vector3 origin,
            EntityRoot excludedRoot,
            bool requireInteractionTarget,
            out EntityRoot targetRoot)
        {
            targetRoot = null;
            var bestDistanceSqr = float.MaxValue;

            for (var index = 0; index < _shelves.Count; index++)
            {
                var shelf = _shelves[index];
                if (IsShelfCandidateValid(shelf, productId, excludedRoot, requireInteractionTarget, out var root) == false)
                    continue;

                var distanceSqr = (root.transform.position - origin).sqrMagnitude;
                if (distanceSqr >= bestDistanceSqr)
                    continue;

                bestDistanceSqr = distanceSqr;
                targetRoot = root;
            }

            return targetRoot != false;
        }

        public bool TryFindCheckout(
            Vector3 origin,
            EntityRoot excludedRoot,
            bool requireInteractionTarget,
            out EntityRoot targetRoot)
        {
            targetRoot = null;
            var bestDistanceSqr = float.MaxValue;

            for (var index = 0; index < _checkouts.Count; index++)
            {
                var checkout = _checkouts[index];
                if (IsCandidateValid(checkout, excludedRoot, requireInteractionTarget, out var root) == false)
                    continue;

                var distanceSqr = (root.transform.position - origin).sqrMagnitude;
                if (distanceSqr >= bestDistanceSqr)
                    continue;

                bestDistanceSqr = distanceSqr;
                targetRoot = root;
            }

            return targetRoot != false;
        }

        public bool TryFindExit(
            Vector3 origin,
            EntityRoot excludedRoot,
            bool requireInteractionTarget,
            out EntityRoot targetRoot)
        {
            targetRoot = null;
            var bestDistanceSqr = float.MaxValue;

            for (var index = 0; index < _exits.Count; index++)
            {
                var exitPoint = _exits[index];
                if (IsCandidateValid(exitPoint, excludedRoot, requireInteractionTarget, out var root) == false)
                    continue;

                var distanceSqr = (root.transform.position - origin).sqrMagnitude;
                if (distanceSqr >= bestDistanceSqr)
                    continue;

                bestDistanceSqr = distanceSqr;
                targetRoot = root;
            }

            return targetRoot != false;
        }

        private static bool IsShelfCandidateValid(
            ShelfProductPart shelf,
            ProductId productId,
            EntityRoot excludedRoot,
            bool requireInteractionTarget,
            out EntityRoot root)
        {
            root = null;

            if (shelf == false || shelf.IsActive == false)
                return false;

            if (shelf.HasStock == false || shelf.ProductId != productId)
                return false;

            return IsCandidateValid(shelf, excludedRoot, requireInteractionTarget, out root);
        }

        private static bool IsCandidateValid(
            EntityComponent component,
            EntityRoot excludedRoot,
            bool requireInteractionTarget,
            out EntityRoot root)
        {
            root = null;

            if (component == false || component.IsActive == false)
                return false;

            root = component.OwnerRoot;
            if (root == false || root == excludedRoot)
                return false;

            if (requireInteractionTarget == false)
                return true;

            return root.TryFindOwnedComponent(out InteractionTargetPart target) && target.IsActive;
        }

        private static void Register<TComponent>(List<TComponent> items, TComponent component)
            where TComponent : EntityComponent
        {
            if (component == false)
                throw new ArgumentNullException(nameof(component));

            if (items.Contains(component) == false)
                items.Add(component);
        }

        private static void Unregister<TComponent>(List<TComponent> items, TComponent component)
            where TComponent : EntityComponent
        {
            if (component == false)
                return;

            items.Remove(component);
        }
    }
}
