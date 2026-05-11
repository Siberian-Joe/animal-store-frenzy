using System;
using System.Collections.Generic;
using Game.World.EntityRuntime;
using Game.World.Features.InteractionTarget;
using Game.World.Inventory;
using Game.World.Shop;
using Game.World.Shop.Checkouts;
using Game.World.Shop.Exits;
using Game.World.Shop.Shelves;
using UnityEngine;

namespace Game.World.UtilityAi
{
    public sealed class ShopUtilityOpportunityLocator :
        IShopUtilityOpportunityLocator,
        IShopInteractionLocator,
        IShopInteractionRegistry
    {
        private readonly Dictionary<ItemId, List<ShelfOpportunityEntry>> _shelvesByItem = new();
        private readonly List<CheckoutOpportunityEntry> _checkouts = new(4);
        private readonly List<ExitOpportunityEntry> _exits = new(4);

        public void Register(ShelfStockPart shelf)
        {
            if (shelf == false)
                throw new ArgumentNullException(nameof(shelf));

            var entry = CreateShelfEntry(shelf);

            if (_shelvesByItem.TryGetValue(entry.ItemId, out var entries) == false)
            {
                entries = new List<ShelfOpportunityEntry>(4);
                _shelvesByItem.Add(entry.ItemId, entries);
            }

            if (ContainsShelf(entries, shelf) == false)
                entries.Add(entry);
        }

        public void Unregister(ShelfStockPart shelf)
        {
            if (shelf == false)
                return;

            if (_shelvesByItem.TryGetValue(shelf.ItemId, out var entries) == false)
                return;

            for (var index = entries.Count - 1; index >= 0; index--)
            {
                if (entries[index].Shelf == shelf)
                    entries.RemoveAt(index);
            }

            if (entries.Count == 0)
                _shelvesByItem.Remove(shelf.ItemId);
        }

        public void Register(CheckoutCounterPart checkout)
        {
            if (checkout == false)
                throw new ArgumentNullException(nameof(checkout));

            if (ContainsCheckout(checkout))
                return;

            _checkouts.Add(CreateCheckoutEntry(checkout));
        }

        public void Unregister(CheckoutCounterPart checkout)
        {
            if (checkout == false)
                return;

            for (var index = _checkouts.Count - 1; index >= 0; index--)
            {
                if (_checkouts[index].Checkout == checkout)
                    _checkouts.RemoveAt(index);
            }
        }

        public void Register(StoreExitPointPart exitPoint)
        {
            if (exitPoint == false)
                throw new ArgumentNullException(nameof(exitPoint));

            if (ContainsExit(exitPoint))
                return;

            _exits.Add(CreateExitEntry(exitPoint));
        }

        public void Unregister(StoreExitPointPart exitPoint)
        {
            if (exitPoint == false)
                return;

            for (var index = _exits.Count - 1; index >= 0; index--)
            {
                if (_exits[index].ExitPoint == exitPoint)
                    _exits.RemoveAt(index);
            }
        }

        public void CollectShelves(
            ItemId itemId,
            EntityRoot excludedRoot,
            bool requireInteractionTarget,
            List<ShelfOpportunityEntry> results)
        {
            if (results == null)
                throw new ArgumentNullException(nameof(results));

            if (_shelvesByItem.TryGetValue(itemId, out var entries) == false)
                return;

            for (var index = 0; index < entries.Count; index++)
            {
                var entry = entries[index];

                if (IsShelfEntryValid(entry, excludedRoot, requireInteractionTarget) == false)
                    continue;

                results.Add(entry);
            }
        }

        public void CollectCheckouts(
            EntityRoot excludedRoot,
            bool requireInteractionTarget,
            List<CheckoutOpportunityEntry> results)
        {
            if (results == null)
                throw new ArgumentNullException(nameof(results));

            for (var index = 0; index < _checkouts.Count; index++)
            {
                var entry = _checkouts[index];

                if (IsCheckoutEntryValid(entry, excludedRoot, requireInteractionTarget) == false)
                    continue;

                results.Add(entry);
            }
        }

        public void CollectExits(
            EntityRoot excludedRoot,
            bool requireInteractionTarget,
            List<ExitOpportunityEntry> results)
        {
            if (results == null)
                throw new ArgumentNullException(nameof(results));

            for (var index = 0; index < _exits.Count; index++)
            {
                var entry = _exits[index];

                if (IsExitEntryValid(entry, excludedRoot, requireInteractionTarget) == false)
                    continue;

                results.Add(entry);
            }
        }

        public bool TryFindShelf(
            ItemId itemId,
            Vector3 origin,
            EntityRoot excludedRoot,
            bool requireInteractionTarget,
            out EntityRoot targetRoot)
        {
            targetRoot = null;
            var bestDistanceSqr = float.MaxValue;

            if (_shelvesByItem.TryGetValue(itemId, out var entries) == false)
                return false;

            for (var index = 0; index < entries.Count; index++)
            {
                var entry = entries[index];

                if (IsShelfEntryValid(entry, excludedRoot, requireInteractionTarget) == false)
                    continue;

                var distanceSqr = (entry.Root.transform.position - origin).sqrMagnitude;
                if (distanceSqr >= bestDistanceSqr)
                    continue;

                bestDistanceSqr = distanceSqr;
                targetRoot = entry.Root;
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
                var entry = _checkouts[index];

                if (IsCheckoutEntryValid(entry, excludedRoot, requireInteractionTarget) == false)
                    continue;

                var distanceSqr = (entry.Root.transform.position - origin).sqrMagnitude;
                if (distanceSqr >= bestDistanceSqr)
                    continue;

                bestDistanceSqr = distanceSqr;
                targetRoot = entry.Root;
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
                var entry = _exits[index];

                if (IsExitEntryValid(entry, excludedRoot, requireInteractionTarget) == false)
                    continue;

                var distanceSqr = (entry.Root.transform.position - origin).sqrMagnitude;
                if (distanceSqr >= bestDistanceSqr)
                    continue;

                bestDistanceSqr = distanceSqr;
                targetRoot = entry.Root;
            }

            return targetRoot != false;
        }

        private static ShelfOpportunityEntry CreateShelfEntry(ShelfStockPart shelf)
        {
            var root = shelf.OwnerRoot;
            root.TryFindOwnedComponent(out InteractionTargetPart interactionTarget);
            return new ShelfOpportunityEntry(shelf, root, interactionTarget);
        }

        private static CheckoutOpportunityEntry CreateCheckoutEntry(CheckoutCounterPart checkout)
        {
            var root = checkout.OwnerRoot;
            root.TryFindOwnedComponent(out InteractionTargetPart interactionTarget);
            return new CheckoutOpportunityEntry(checkout, root, interactionTarget);
        }

        private static ExitOpportunityEntry CreateExitEntry(StoreExitPointPart exitPoint)
        {
            var root = exitPoint.OwnerRoot;
            root.TryFindOwnedComponent(out InteractionTargetPart interactionTarget);
            return new ExitOpportunityEntry(exitPoint, root, interactionTarget);
        }

        private static bool IsShelfEntryValid(
            ShelfOpportunityEntry entry,
            EntityRoot excludedRoot,
            bool requireInteractionTarget)
        {
            if (entry.Shelf == false || entry.Shelf.IsActive == false)
                return false;

            if (entry.Shelf.HasStock == false)
                return false;

            return IsCommonEntryValid(
                entry.Root,
                entry.InteractionTarget,
                excludedRoot,
                requireInteractionTarget);
        }

        private static bool IsCheckoutEntryValid(
            CheckoutOpportunityEntry entry,
            EntityRoot excludedRoot,
            bool requireInteractionTarget)
        {
            if (entry.Checkout == false || entry.Checkout.IsActive == false)
                return false;

            return IsCommonEntryValid(
                entry.Root,
                entry.InteractionTarget,
                excludedRoot,
                requireInteractionTarget);
        }

        private static bool IsExitEntryValid(
            ExitOpportunityEntry entry,
            EntityRoot excludedRoot,
            bool requireInteractionTarget)
        {
            if (entry.ExitPoint == false || entry.ExitPoint.IsActive == false)
                return false;

            return IsCommonEntryValid(
                entry.Root,
                entry.InteractionTarget,
                excludedRoot,
                requireInteractionTarget);
        }

        private static bool IsCommonEntryValid(
            EntityRoot root,
            InteractionTargetPart interactionTarget,
            EntityRoot excludedRoot,
            bool requireInteractionTarget)
        {
            if (root == false || root == excludedRoot)
                return false;

            if (requireInteractionTarget == false)
                return true;

            return interactionTarget != false && interactionTarget.IsActive;
        }

        private static bool ContainsShelf(
            List<ShelfOpportunityEntry> entries,
            ShelfStockPart shelf)
        {
            for (var index = 0; index < entries.Count; index++)
            {
                if (entries[index].Shelf == shelf)
                    return true;
            }

            return false;
        }

        private bool ContainsCheckout(CheckoutCounterPart checkout)
        {
            for (var index = 0; index < _checkouts.Count; index++)
            {
                if (_checkouts[index].Checkout == checkout)
                    return true;
            }

            return false;
        }

        private bool ContainsExit(StoreExitPointPart exitPoint)
        {
            for (var index = 0; index < _exits.Count; index++)
            {
                if (_exits[index].ExitPoint == exitPoint)
                    return true;
            }

            return false;
        }
    }
}