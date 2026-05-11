using System;
using System.Collections.Generic;
using Game.World.Interactions;
using Game.World.Persistence;
using R3;
using UnityEngine;

namespace Game.World.Inventory
{
    [DisallowMultipleComponent]
    public sealed class InventoryPart : StatefulEntityComponent<InventoryState>, IInventory, IInventoryRole,
        IInteractionRoleProvider
    {
        private readonly List<ItemStack> _itemsSnapshot = new();
        private readonly Subject<Unit> _changed = new();

        public override int ActivationOrder => 320;

        public IReadOnlyList<ItemStack> Items
        {
            get
            {
                RebuildSnapshot();
                return _itemsSnapshot;
            }
        }

        public Observable<Unit> Changed => _changed;
        public IInventory Inventory => this;

        public void RegisterRoles(InteractionRoleRegistry registry)
        {
            if (registry == null)
                throw new ArgumentNullException(nameof(registry));

            registry.Add<IInventoryRole>(this);
        }

        protected override StateSlotKey GetStateSlotKey() => StateSlotKey.For(typeof(InventoryState), "inventory");

        protected override void RestoreState(InventoryState state) => Normalize(state);

        protected override void InitializeFreshState(InventoryState state)
        {
            state.Items ??= new List<InventoryItemState>();
            Normalize(state);
        }

        protected override void ApplyBoundState(InventoryState state) => Normalize(state);

        public int GetAmount(ItemId itemId)
        {
            var item = FindStateItem(itemId);
            return item?.Amount ?? 0;
        }

        public bool Has(ItemStack stack) => GetAmount(stack.ItemId) >= stack.Amount;

        public bool Has(IReadOnlyList<ItemStack> cost)
        {
            if (cost == null)
                throw new ArgumentNullException(nameof(cost));

            foreach (var stack in cost)
            {
                if (Has(stack) == false)
                    return false;
            }

            return true;
        }

        public bool CanAccept(ItemStack stack) => stack.Amount > 0;

        public bool CanPay(IReadOnlyList<ItemStack> cost) => Has(cost);

        public void Add(ItemStack stack)
        {
            var stateItem = FindStateItem(stack.ItemId);
            if (stateItem == null)
            {
                State.Items.Add(new InventoryItemState
                {
                    ItemId = stack.ItemId.Value,
                    Amount = stack.Amount
                });
            }
            else
            {
                stateItem.Amount += stack.Amount;
            }

            Normalize(State);
            _changed.OnNext(Unit.Default);
        }

        public bool TryRemove(ItemStack stack)
        {
            if (Has(stack) == false)
                return false;

            var stateItem = FindStateItem(stack.ItemId);
            stateItem.Amount -= stack.Amount;
            Normalize(State);
            _changed.OnNext(Unit.Default);
            return true;
        }

        public bool TryRemove(IReadOnlyList<ItemStack> cost)
        {
            if (cost == null)
                throw new ArgumentNullException(nameof(cost));

            if (Has(cost) == false)
                return false;

            foreach (var stack in cost)
                TryRemove(stack);

            return true;
        }

        protected override void OnDestroy()
        {
            _changed.Dispose();
            base.OnDestroy();
        }

        private InventoryItemState FindStateItem(ItemId itemId)
        {
            foreach (var item in State.Items)
            {
                if (item == null || string.IsNullOrWhiteSpace(item.ItemId))
                    continue;

                if (new ItemId(item.ItemId) == itemId)
                    return item;
            }

            return null;
        }

        private void RebuildSnapshot()
        {
            _itemsSnapshot.Clear();

            foreach (var item in State.Items)
            {
                if (item == null || string.IsNullOrWhiteSpace(item.ItemId) || item.Amount <= 0)
                    continue;

                _itemsSnapshot.Add(new ItemStack(new ItemId(item.ItemId), item.Amount));
            }
        }

        private static void Normalize(InventoryState state)
        {
            state.Items ??= new List<InventoryItemState>();

            for (var index = state.Items.Count - 1; index >= 0; index--)
            {
                var item = state.Items[index];
                if (item == null || string.IsNullOrWhiteSpace(item.ItemId) || item.Amount <= 0)
                {
                    state.Items.RemoveAt(index);
                    continue;
                }

                item.ItemId = item.ItemId.Trim();
            }

            for (var left = 0; left < state.Items.Count; left++)
            {
                var leftItem = state.Items[left];
                for (var right = state.Items.Count - 1; right > left; right--)
                {
                    var rightItem = state.Items[right];
                    if (string.Equals(leftItem.ItemId, rightItem.ItemId, StringComparison.Ordinal) == false)
                        continue;

                    leftItem.Amount += rightItem.Amount;
                    state.Items.RemoveAt(right);
                }
            }
        }
    }
}