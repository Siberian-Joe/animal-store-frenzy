using System;
using System.Collections.Generic;
using Game.World.Shop.Customers;
using UnityEngine;
using Zenject;

namespace Game.World.UtilityAi
{
    public sealed class LeaveStoreActionPart : CustomerInteractionActionPart<LeaveStoreOption>
    {
        [Header("Leave Store")] [SerializeField]
        private CustomerNeedsPart _customerNeeds;

        [SerializeField] private CustomerBasketPart _customerBasket;
        [SerializeField] private CustomerCheckoutProgressPart _checkoutProgress;
        [SerializeField, Min(0.1f)] private float _maxRelevantDistance = 12f;
        [SerializeField] private bool _requireInteractionTarget = true;

        private readonly List<ExitOpportunityEntry> _exitBuffer = new(4);
        private IShopUtilityOpportunityLocator _opportunityLocator;
        private ICustomerNeedResolution _needResolution;

        [Inject]
        public void ConstructLocator(
            IShopUtilityOpportunityLocator opportunityLocator,
            ICustomerNeedResolution needResolution)
        {
            _opportunityLocator = opportunityLocator;
            _needResolution = needResolution;
        }

        protected override void OnActionActivated()
        {
            base.OnActionActivated();

            _customerNeeds ??= OwnerRoot.FindOwnedComponent<CustomerNeedsPart>();
            _customerBasket ??= OwnerRoot.FindOwnedComponent<CustomerBasketPart>();
            _checkoutProgress ??= OwnerRoot.FindOwnedComponent<CustomerCheckoutProgressPart>();

            if (_customerNeeds == false)
                throw new InvalidOperationException($"{GetType().Name} requires {nameof(CustomerNeedsPart)}.");

            if (_customerBasket == false)
                throw new InvalidOperationException($"{GetType().Name} requires {nameof(CustomerBasketPart)}.");

            if (_checkoutProgress == false)
                throw new InvalidOperationException($"{GetType().Name} requires {nameof(CustomerCheckoutProgressPart)}.");

            if (_opportunityLocator == null)
                throw new InvalidOperationException(
                    $"{GetType().Name} requires {nameof(IShopUtilityOpportunityLocator)} injection.");

            if (_needResolution == null)
                throw new InvalidOperationException(
                    $"{GetType().Name} requires {nameof(ICustomerNeedResolution)} injection.");
        }

        public override void CollectOptions(List<IUtilityOption> options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (_customerBasket.HasItems)
                return;

            if (_checkoutProgress.IsWaitingForCheckout)
                return;

            if (_needResolution.HasPendingShelfVisit(OwnerRoot, _customerNeeds))
                return;

            _exitBuffer.Clear();

            _opportunityLocator.CollectExits(
                OwnerRoot,
                _requireInteractionTarget,
                _exitBuffer);

            foreach (var entry in _exitBuffer)
            {
                if (entry.ExitPoint == false || entry.Root == false || entry.HasInteractionTarget == false)
                    continue;

                var reachability = EvaluateTarget(
                    entry.InteractionTarget.ResolveApproachPoint(InteractionActor),
                    out var navigationTarget);

                if (reachability.IsReachable == false)
                    continue;

                options.Add(new LeaveStoreOption(
                    entry.ExitPoint,
                    entry.InteractionTarget,
                    navigationTarget,
                    reachability.NormalizeDistance(_maxRelevantDistance)));
            }
        }
    }
}
