using System;
using Game.World.Persistence;
using Game.World.Shop.Products;
using UnityEngine;
using Zenject;

namespace Game.World.Shop.Shelves
{
    [DisallowMultipleComponent]
    public sealed class ShelfProductPart :
        StatefulEntityComponent<ShelfInventoryState>, IShelfProductSource
    {
        [Header("Shelf Product")] [SerializeField]
        private ProductDefinition _product;

        [SerializeField, Min(0)] private int _initialQuantity = 5;

        private IShopInteractionRegistry _shopInteractionRegistry;

        public override int ActivationOrder => 240;

        public ProductId ProductId => _product.ProductId;

        public int CurrentQuantity => Mathf.Max(0, State.CurrentQuantity);

        public bool HasStock => CurrentQuantity > 0;

        protected override StateSlotKey GetStateSlotKey() =>
            StateSlotKey.For(typeof(ShelfInventoryState), "shelf-inventory");

        protected override void RestoreState(ShelfInventoryState state)
        {
        }

        protected override void InitializeFreshState(ShelfInventoryState state)
        {
            state.CurrentQuantity = Mathf.Max(0, _initialQuantity);
        }

        [Inject]
        public void Construct(IShopInteractionRegistry shopInteractionRegistry) =>
            _shopInteractionRegistry = shopInteractionRegistry;

        protected override void OnStateActivated()
        {
            if (_product == false)
            {
                throw new InvalidOperationException(
                    $"{nameof(ShelfProductPart)} on '{name}' requires a {nameof(ProductDefinition)} reference.");
            }

            if (State.CurrentQuantity < 0)
                State.CurrentQuantity = 0;

            if (_shopInteractionRegistry == null)
            {
                throw new InvalidOperationException(
                    $"{nameof(ShelfProductPart)} on '{name}' requires {nameof(IShopInteractionRegistry)} injection.");
            }

            _shopInteractionRegistry.Register(this);
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

            if (State.CurrentQuantity < quantity)
                return false;

            State.CurrentQuantity -= quantity;
            return true;
        }

        public void Restock(int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(quantity),
                    quantity,
                    "Restock quantity must be positive.");
            }

            State.CurrentQuantity += quantity;
        }

        protected override void OnDeactivate() => _shopInteractionRegistry?.Unregister(this);
    }
}