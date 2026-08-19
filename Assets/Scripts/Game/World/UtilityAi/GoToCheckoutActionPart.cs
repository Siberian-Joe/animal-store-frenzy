using System;
using System.Collections.Generic;
using Game.World.Shop.Customers;
using UnityEngine;
using Zenject;

namespace Game.World.UtilityAi
{
    public sealed class GoToCheckoutActionPart : CustomerInteractionActionPart<CheckoutOption>
    {
        [Header("Go To Checkout")] [SerializeField]
        private CustomerNeedsPart _customerNeeds;

        [SerializeField] private CustomerBasketPart _customerBasket;
        [SerializeField] private CustomerCheckoutProgressPart _checkoutProgress;
        [SerializeField, Min(0.1f)] private float _maxRelevantDistance = 12f;
        [SerializeField] private bool _requireInteractionTarget = true;

        private readonly List<CheckoutOpportunityEntry> _checkoutBuffer = new(4);
        private IShopUtilityOpportunityLocator _opportunityLocator;

        [Inject]
        public void ConstructLocator(IShopUtilityOpportunityLocator opportunityLocator) =>
            _opportunityLocator = opportunityLocator;

        protected override void OnActionActivated()
        {
            base.OnActionActivated();

            _customerNeeds ??= OwnerRoot.FindOwnedComponent<CustomerNeedsPart>();
            _customerBasket ??= OwnerRoot.FindOwnedComponent<CustomerBasketPart>();
            _checkoutProgress ??= OwnerRoot.FindOwnedComponent<CustomerCheckoutProgressPart>();

            if (_customerNeeds == false)
                throw new InvalidOperationException(
                    $"{nameof(GoToCheckoutActionPart)} requires {nameof(CustomerNeedsPart)}.");

            if (_customerBasket == false)
                throw new InvalidOperationException(
                    $"{nameof(GoToCheckoutActionPart)} requires {nameof(CustomerBasketPart)}.");

            if (_checkoutProgress == false)
                throw new InvalidOperationException(
                    $"{nameof(GoToCheckoutActionPart)} requires {nameof(CustomerCheckoutProgressPart)}.");

            if (_opportunityLocator == null)
                throw new InvalidOperationException(
                    $"{nameof(GoToCheckoutActionPart)} requires {nameof(IShopUtilityOpportunityLocator)} injection.");
        }

        public override void CollectOptions(List<IUtilityOption> options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (_customerNeeds.HasActiveNeeds)
                return;

            if (_customerBasket.HasItems == false)
                return;

            if (_checkoutProgress.IsCheckoutCompleted || _checkoutProgress.IsWaitingForCheckout)
                return;

            _checkoutBuffer.Clear();

            _opportunityLocator.CollectCheckouts(
                OwnerRoot,
                _requireInteractionTarget,
                _checkoutBuffer);

            foreach (var entry in _checkoutBuffer)
            {
                if (entry.Checkout == false || entry.Root == false || entry.HasInteractionTarget == false)
                    continue;

                var reachability = EvaluateTarget(
                    entry.ApproachPoint,
                    out var navigationTarget);

                if (reachability.IsReachable == false)
                    continue;

                options.Add(new CheckoutOption(
                    entry.Checkout,
                    entry.InteractionTarget,
                    navigationTarget,
                    reachability.NormalizeDistance(_maxRelevantDistance)));
            }
        }
    }
}