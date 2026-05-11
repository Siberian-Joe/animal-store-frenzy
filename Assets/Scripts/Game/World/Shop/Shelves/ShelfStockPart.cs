using System;
using Game.World.Inventory;
using Game.World.Persistence;
using R3;
using UnityEngine;
using Zenject;

namespace Game.World.Shop.Shelves
{
    [DisallowMultipleComponent]
    public sealed class ShelfStockPart : StatefulEntityComponent<ShelfStockState>, IShelfStock, IShelfItemSource
    {
        [Header("Shelf Stock")] [SerializeField]
        private ItemDefinition _acceptedItem;

        [SerializeField, Min(1)] private int _capacity = 1;
        [SerializeField] private GameObject[] _emptyVisualObjects;
        [SerializeField] private GameObject[] _stockedVisualObjects;

        [Inject] private readonly IShopInteractionRegistry _shopInteractionRegistry;

        private readonly Subject<Unit> _changed = new();

        public override int ActivationOrder => 300;
        public ItemId AcceptedItemId => _acceptedItem.Id;
        public ItemId ItemId => AcceptedItemId;
        public int CurrentAmount => Mathf.Max(0, State.Amount);
        public int CurrentQuantity => CurrentAmount;
        public int Capacity => Mathf.Max(1, _capacity);
        public int AvailableCapacity => Mathf.Max(0, Capacity - CurrentAmount);
        public bool HasStock => CurrentQuantity > 0;
        public Observable<Unit> Changed => _changed;

        protected override StateSlotKey GetStateSlotKey() => StateSlotKey.For(typeof(ShelfStockState), "shelf-stock");

        protected override void RestoreState(ShelfStockState state) => Normalize(state);

        protected override void InitializeFreshState(ShelfStockState state)
        {
            state.ItemId = _acceptedItem ? _acceptedItem.Id.Value : null;
            state.Amount = 0;
            Normalize(state);
        }

        protected override void ApplyBoundState(ShelfStockState state)
        {
            Validate();
            Normalize(state);
            ApplyVisuals();
        }

        protected override void OnStateActivated() => _shopInteractionRegistry?.Register(this);

        protected override void OnDeactivate()
        {
            _shopInteractionRegistry?.Unregister(this);
            base.OnDeactivate();
        }

        public bool CanStock(ItemStack stack)
        {
            Validate();

            if (stack.ItemId != AcceptedItemId)
                return false;

            if (stack.Amount <= 0)
                return false;

            return stack.Amount <= AvailableCapacity;
        }

        public void Stock(ItemStack stack)
        {
            if (CanStock(stack) == false)
            {
                throw new InvalidOperationException(
                    $"Shelf '{name}' cannot stock '{stack}'. Accepted item: '{AcceptedItemId}', amount: {CurrentAmount}/{Capacity}.");
            }

            State.ItemId = stack.ItemId.Value;
            State.Amount += stack.Amount;
            ApplyVisuals();
            _changed.OnNext(Unit.Default);
        }

        public bool TryTake(int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(quantity),
                    quantity,
                    "Take quantity must be positive.");
            }

            if (CurrentQuantity < quantity)
                return false;

            State.Amount -= quantity;
            ApplyVisuals();
            _changed.OnNext(Unit.Default);
            return true;
        }

        public bool Contains(ItemId itemId, int requiredAmount)
        {
            if (requiredAmount <= 0)
                throw new ArgumentOutOfRangeException(nameof(requiredAmount), requiredAmount,
                    "Required amount must be positive.");

            return AcceptedItemId == itemId && CurrentAmount >= requiredAmount;
        }

        protected override void OnDestroy()
        {
            _changed.Dispose();
            base.OnDestroy();
        }

        private void Validate()
        {
            if (_acceptedItem == false)
                throw new InvalidOperationException(
                    $"{nameof(ShelfStockPart)} on '{name}' requires accepted item definition.");

            _acceptedItem.Validate();
        }

        private void Normalize(ShelfStockState state)
        {
            Validate();
            state.ItemId = _acceptedItem.Id.Value;
            state.Amount = Mathf.Clamp(state.Amount, 0, Capacity);
        }

        private void ApplyVisuals()
        {
            var hasStock = CurrentAmount > 0;
            SetActive(_emptyVisualObjects, hasStock == false);
            SetActive(_stockedVisualObjects, hasStock);
        }

        private static void SetActive(GameObject[] objects, bool isActive)
        {
            if (objects == null)
                return;

            foreach (var target in objects)
            {
                if (target)
                    target.SetActive(isActive);
            }
        }
    }
}