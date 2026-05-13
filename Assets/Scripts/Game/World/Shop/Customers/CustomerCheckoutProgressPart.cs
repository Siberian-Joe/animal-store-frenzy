using Game.World.Persistence;
using UnityEngine;

namespace Game.World.Shop.Customers
{
    [DisallowMultipleComponent]
    public sealed class CustomerCheckoutProgressPart :
        StatefulEntityComponent<CustomerCheckoutProgressState>,
        ICustomerCheckoutProgressWriter
    {
        public override int ActivationOrder => 325;

        public bool IsCheckoutCompleted => State.IsCheckoutCompleted;

        protected override StateSlotKey GetStateSlotKey() =>
            StateSlotKey.For(typeof(CustomerCheckoutProgressState), "customer-checkout-progress");

        protected override void RestoreState(CustomerCheckoutProgressState state)
        {
        }

        protected override void InitializeFreshState(CustomerCheckoutProgressState state) =>
            state.IsCheckoutCompleted = false;

        public void MarkCheckoutCompleted() => State.IsCheckoutCompleted = true;

        public void ResetCheckoutProgress() => State.IsCheckoutCompleted = false;
    }
}