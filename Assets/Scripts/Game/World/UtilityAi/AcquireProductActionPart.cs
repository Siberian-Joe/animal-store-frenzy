using System;
using System.Collections.Generic;
using Game.World.Inventory;
using Game.World.Shop.Customers;
using UnityEngine;
using Zenject;

namespace Game.World.UtilityAi
{
    public sealed class AcquireProductActionPart : CustomerInteractionActionPart<AcquireProductOption>
    {
        [Header("Acquire Product")] [SerializeField]
        private CustomerNeedsPart _customerNeeds;

        [SerializeField, Min(0.1f)] private float _maxRelevantDistance = 12f;
        [SerializeField] private bool _requireInteractionTarget = true;

        private readonly List<ShelfOpportunityEntry> _shelfBuffer = new(8);
        private IShopUtilityOpportunityLocator _opportunityLocator;

        [Inject]
        public void ConstructLocator(IShopUtilityOpportunityLocator opportunityLocator) =>
            _opportunityLocator = opportunityLocator;

        protected override void OnActionActivated()
        {
            base.OnActionActivated();

            _customerNeeds ??= OwnerRoot.FindOwnedComponent<CustomerNeedsPart>();

            if (_customerNeeds == false)
                throw new InvalidOperationException($"{GetType().Name} requires {nameof(CustomerNeedsPart)}.");

            if (_opportunityLocator == null)
            {
                throw new InvalidOperationException(
                    $"{GetType().Name} requires {nameof(IShopUtilityOpportunityLocator)} injection.");
            }
        }

        public override void CollectOptions(List<IUtilityOption> options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            var needs = _customerNeeds.Needs;
            if (needs == null || needs.Count == 0)
                return;

            foreach (var need in needs)
            {
                if (need == null || need.Intensity <= 0f)
                    continue;

                if (string.IsNullOrWhiteSpace(need.ItemId))
                    continue;

                var itemId = new ItemId(need.ItemId);
                _shelfBuffer.Clear();

                _opportunityLocator.CollectShelves(
                    itemId,
                    OwnerRoot,
                    _requireInteractionTarget,
                    _shelfBuffer);

                if (_shelfBuffer.Count <= 0)
                    continue;

                var needHandle = new CustomerNeedHandle(
                    need.NeedId,
                    itemId,
                    need.Intensity);

                foreach (var entry in _shelfBuffer)
                {
                    if (entry.Shelf == false || entry.Root == false || entry.HasInteractionTarget == false)
                        continue;

                    var reachability = EvaluateTarget(
                        entry.InteractionTarget.ResolveApproachPoint(InteractionActor),
                        out var navigationTarget);

                    if (reachability.IsReachable == false)
                        continue;

                    options.Add(new AcquireProductOption(
                        needHandle,
                        entry.Shelf,
                        entry.InteractionTarget,
                        navigationTarget,
                        reachability.NormalizeDistance(_maxRelevantDistance)));
                }
            }
        }
    }
}