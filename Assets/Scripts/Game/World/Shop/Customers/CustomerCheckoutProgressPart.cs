using System;
using Game.World.Persistence;
using UnityEngine;
using EntityId = Game.World.EntityRuntime.EntityId;

namespace Game.World.Shop.Customers
{
    [DisallowMultipleComponent]
    public sealed class CustomerCheckoutProgressPart :
        StatefulEntityComponent<CustomerCheckoutProgressState>,
        ICustomerCheckoutProgressWriter
    {
        public override int ActivationOrder => 325;

        public bool IsCheckoutCompleted => State.IsCheckoutCompleted;

        public bool IsWaitingForCheckout =>
            IsCheckoutCompleted == false && string.IsNullOrWhiteSpace(State.WaitingCheckoutId) == false;

        protected override StateSlotKey GetStateSlotKey() =>
            StateSlotKey.For(typeof(CustomerCheckoutProgressState), "customer-checkout-progress");

        protected override void RestoreState(CustomerCheckoutProgressState state) => Normalize(state);

        protected override void InitializeFreshState(CustomerCheckoutProgressState state)
        {
            state.IsCheckoutCompleted = false;
            state.WaitingCheckoutId = null;
        }

        protected override void ApplyBoundState(CustomerCheckoutProgressState state) => Normalize(state);

        public bool IsWaitingAt(EntityId checkoutId) =>
            IsWaitingForCheckout &&
            string.Equals(State.WaitingCheckoutId, checkoutId.Value, StringComparison.Ordinal);

        public void MarkWaitingForCheckout(EntityId checkoutId)
        {
            if (IsCheckoutCompleted)
                throw new InvalidOperationException("Cannot wait for checkout after checkout is completed.");

            State.WaitingCheckoutId = checkoutId.Value;
        }

        public void MarkCheckoutCompleted()
        {
            State.IsCheckoutCompleted = true;
            State.WaitingCheckoutId = null;
        }

        public void ResetCheckoutProgress()
        {
            State.IsCheckoutCompleted = false;
            State.WaitingCheckoutId = null;
        }

        private static void Normalize(CustomerCheckoutProgressState state)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));

            if (state.IsCheckoutCompleted)
            {
                state.WaitingCheckoutId = null;
                return;
            }

            state.WaitingCheckoutId = string.IsNullOrWhiteSpace(state.WaitingCheckoutId)
                ? null
                : state.WaitingCheckoutId.Trim();
        }
    }
}