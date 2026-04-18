using System;
using System.Collections.Generic;
using Game.World.Interactions;
using Game.World.Persistence;
using UnityEngine;

namespace Game.World.Shop.Customers
{
    [DisallowMultipleComponent]
    public sealed class CustomerNeedsPart :
        StatefulEntityComponent<CustomerNeedsState>,
        ICustomerNeeds,
        ICustomerShoppingRole,
        IInteractionRoleProvider
    {
        public override int ActivationOrder => 320;

        public bool HasActiveNeeds => ActiveNeedCount > 0;

        public int ActiveNeedCount
        {
            get
            {
                if (State.Needs == null)
                    return 0;

                var count = 0;

                foreach (var need in State.Needs)
                {
                    if (IsActiveNeed(need))
                        count++;
                }

                return count;
            }
        }

        public IReadOnlyList<CustomerNeedState> Needs => State.Needs;

        protected override StateSlotKey GetStateSlotKey() =>
            StateSlotKey.For(typeof(CustomerNeedsState), "customer-needs");

        protected override void RestoreState(CustomerNeedsState state)
        {
        }

        protected override void InitializeFreshState(CustomerNeedsState state) => state.Needs.Clear();

        protected override void OnStateActivated()
        {
            State.Needs ??= new List<CustomerNeedState>();
            SanitizeState();
        }

        public void ReplaceNeeds(IEnumerable<CustomerNeedState> needs)
        {
            if (needs == null)
                throw new ArgumentNullException(nameof(needs));

            State.Needs.Clear();

            foreach (var need in needs)
            {
                if (need == null)
                    continue;

                var clone = need.DeepClone();

                if (string.IsNullOrWhiteSpace(clone.NeedId))
                    clone.NeedId = Guid.NewGuid().ToString("N");

                State.Needs.Add(clone);
            }

            SanitizeState();
        }

        public bool WantsProduct(ProductId productId)
        {
            if (State.Needs == null || State.Needs.Count == 0)
                return false;

            foreach (var need in State.Needs)
            {
                if (IsActiveNeed(need) == false)
                    continue;

                if (string.Equals(need.ProductId, productId.Value, StringComparison.Ordinal))
                    return true;
            }

            return false;
        }

        public bool TrySatisfyProductNeed(ProductId productId, float satisfactionAmount = 1f)
        {
            if (satisfactionAmount <= 0f)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(satisfactionAmount),
                    satisfactionAmount,
                    "Satisfaction amount must be positive.");
            }

            if (State.Needs == null || State.Needs.Count == 0)
                return false;

            foreach (var need in State.Needs)
            {
                if (IsActiveNeed(need) == false)
                    continue;

                if (string.Equals(need.ProductId, productId.Value, StringComparison.Ordinal) == false)
                    continue;

                need.Intensity = Mathf.Max(0f, need.Intensity - satisfactionAmount);
                SanitizeState();
                return true;
            }

            return false;
        }

        public void RegisterRoles(InteractionRoleRegistry registry)
        {
            if (registry == null)
                throw new ArgumentNullException(nameof(registry));

            registry.Add<ICustomerShoppingRole>(this);
        }

        private void SanitizeState()
        {
            for (var index = State.Needs.Count - 1; index >= 0; index--)
            {
                var need = State.Needs[index];

                if (need == null)
                {
                    State.Needs.RemoveAt(index);
                    continue;
                }

                need.NeedId = string.IsNullOrWhiteSpace(need.NeedId)
                    ? Guid.NewGuid().ToString("N")
                    : need.NeedId.Trim();

                need.ProductId = string.IsNullOrWhiteSpace(need.ProductId)
                    ? string.Empty
                    : need.ProductId.Trim();

                need.Intensity = Mathf.Clamp01(need.Intensity);

                if (string.IsNullOrWhiteSpace(need.ProductId) || need.Intensity <= 0f)
                    State.Needs.RemoveAt(index);
            }
        }

        private static bool IsActiveNeed(CustomerNeedState need) =>
            need != null &&
            string.IsNullOrWhiteSpace(need.ProductId) == false &&
            need.Intensity > 0f;
    }
}