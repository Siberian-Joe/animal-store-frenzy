using System;
using System.Collections.Generic;
using Game.World.Interactions;
using Game.World.Persistence;
using UnityEngine;

namespace Game.World.Shop.Customers
{
    [DisallowMultipleComponent]
    public sealed class CustomerBasketPart :
        StatefulEntityComponent<CustomerBasketState>,
        ICustomerBasket,
        ICustomerBasketRole,
        IInteractionRoleProvider
    {
        public override int ActivationOrder => 330;

        public bool HasItems => TotalItemCount > 0;

        public int TotalItemCount
        {
            get
            {
                var total = 0;

                if (State.Items == null)
                    return 0;

                foreach (var item in State.Items)
                {
                    if (item is not { Quantity: > 0 })
                        continue;

                    total += item.Quantity;
                }

                return total;
            }
        }

        public int UniqueItemCount
        {
            get
            {
                if (State.Items == null)
                    return 0;

                var total = 0;

                for (var index = 0; index < State.Items.Count; index++)
                {
                    var item = State.Items[index];
                    if (item == null || item.Quantity <= 0 || string.IsNullOrWhiteSpace(item.ProductId))
                        continue;

                    total++;
                }

                return total;
            }
        }

        protected override StateSlotKey GetStateSlotKey() =>
            StateSlotKey.For(typeof(CustomerBasketState), "customer-basket");

        protected override void RestoreState(CustomerBasketState state)
        {
        }

        protected override void InitializeFreshState(CustomerBasketState state)
        {
            state.Items ??= new List<CustomerBasketItemState>();
            state.Items.Clear();
        }

        protected override void OnStateActivated()
        {
            State.Items ??= new List<CustomerBasketItemState>();
            CleanupState();
        }

        public int GetQuantity(ProductId productId)
        {
            if (State.Items == null)
                return 0;

            for (var index = 0; index < State.Items.Count; index++)
            {
                var item = State.Items[index];
                if (item == null || item.Quantity <= 0)
                    continue;

                if (item.Matches(productId))
                    return item.Quantity;
            }

            return 0;
        }

        public void AddProduct(ProductId productId, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity), quantity, "Basket quantity must be positive.");

            State.Items ??= new List<CustomerBasketItemState>();

            for (var index = 0; index < State.Items.Count; index++)
            {
                var item = State.Items[index];
                if (item == null)
                    continue;

                if (item.Matches(productId) == false)
                    continue;

                item.Quantity += quantity;
                return;
            }

            State.Items.Add(new CustomerBasketItemState
            {
                ProductId = productId.Value,
                Quantity = quantity
            });
        }

        public void ClearBasket()
        {
            State.Items ??= new List<CustomerBasketItemState>();
            State.Items.Clear();
        }

        public void RegisterRoles(InteractionRoleRegistry registry)
        {
            if (registry == null)
                throw new ArgumentNullException(nameof(registry));

            registry.Add<ICustomerBasketRole>(this);
        }

        private void CleanupState()
        {
            for (var index = State.Items.Count - 1; index >= 0; index--)
            {
                var item = State.Items[index];

                if (item == null || string.IsNullOrWhiteSpace(item.ProductId) || item.Quantity <= 0)
                    State.Items.RemoveAt(index);
            }
        }
    }
}