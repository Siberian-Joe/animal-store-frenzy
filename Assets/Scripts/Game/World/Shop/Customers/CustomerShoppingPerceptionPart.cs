using System;
using Game.World.EntityRuntime;
using Game.World.Inventory;
using UnityEngine;
using Zenject;

namespace Game.World.Shop.Customers
{
    [DisallowMultipleComponent]
    public sealed class CustomerShoppingPerceptionPart : EntityComponent
    {
        [Header("References")] [SerializeField]
        private CustomerNeedsPart _customerNeeds;

        [Header("Search")] [SerializeField] private bool _requireInteractionTarget = true;
        [SerializeField, Min(0.1f)] private float _maxRelevantDistance = 12f;

        private IShopInteractionLocator _shopInteractionLocator;

        private int _lastEvaluatedFrame = -1;
        private bool _hasBestNeed;
        private BestNeedEvaluation _bestNeed;

        public override int ActivationOrder => 350;

        public bool HasReachableNeed
        {
            get
            {
                EnsureEvaluated();
                return _hasBestNeed;
            }
        }

        public float HighestReachableNeedIntensity
        {
            get
            {
                EnsureEvaluated();
                return _hasBestNeed ? _bestNeed.Intensity : 0f;
            }
        }

        public float BestReachableNeedProximityScore
        {
            get
            {
                EnsureEvaluated();
                return _hasBestNeed ? _bestNeed.ProximityScore : 0f;
            }
        }

        [Inject]
        public void Construct(IShopInteractionLocator shopInteractionLocator)
        {
            _shopInteractionLocator = shopInteractionLocator;
        }

        protected override void OnActivate()
        {
            _customerNeeds ??= OwnerRoot.FindOwnedComponent<CustomerNeedsPart>();

            if (_customerNeeds == false)
            {
                throw new InvalidOperationException(
                    $"{nameof(CustomerShoppingPerceptionPart)} on '{name}' requires {nameof(CustomerNeedsPart)} under the same EntityRoot.");
            }

            if (_shopInteractionLocator == null)
            {
                throw new InvalidOperationException(
                    $"{nameof(CustomerShoppingPerceptionPart)} on '{name}' requires {nameof(IShopInteractionLocator)} injection.");
            }
        }

        public bool TryGetBestReachableNeedTarget(out EntityRoot targetRoot)
        {
            EnsureEvaluated();

            if (_hasBestNeed)
            {
                targetRoot = _bestNeed.TargetRoot;
                return targetRoot != false;
            }

            targetRoot = null;
            return false;
        }

        private void EnsureEvaluated()
        {
            var frame = Time.frameCount;
            if (_lastEvaluatedFrame == frame)
                return;

            _lastEvaluatedFrame = frame;
            RebuildEvaluation();
        }

        private void RebuildEvaluation()
        {
            _hasBestNeed = false;
            _bestNeed = default;

            if (_customerNeeds == false || _shopInteractionLocator == null)
                return;

            var needs = _customerNeeds.Needs;
            if (needs == null || needs.Count == 0)
                return;

            var origin = OwnerRoot ? OwnerRoot.transform.position : transform.position;
            var bestScore = float.MinValue;

            foreach (var need in needs)
            {
                if (need == null || need.Intensity <= 0f)
                    continue;

                if (string.IsNullOrWhiteSpace(need.ItemId))
                    continue;

                var itemId = new ItemId(need.ItemId);

                if (_shopInteractionLocator.TryFindShelf(
                        itemId,
                        origin,
                        OwnerRoot,
                        _requireInteractionTarget,
                        out var targetRoot) == false)
                {
                    continue;
                }

                var distance = Vector3.Distance(origin, targetRoot.transform.position);
                var proximityScore = 1f - Mathf.Clamp01(distance / Mathf.Max(0.1f, _maxRelevantDistance));
                var intensity = Mathf.Clamp01(need.Intensity);
                var finalScore = intensity * proximityScore;

                if (_hasBestNeed && finalScore <= bestScore)
                    continue;

                _bestNeed = new BestNeedEvaluation(
                    need,
                    targetRoot,
                    intensity,
                    proximityScore,
                    finalScore);

                bestScore = finalScore;
                _hasBestNeed = true;
            }
        }

        private readonly struct BestNeedEvaluation
        {
            public CustomerNeedState Need { get; }
            public EntityRoot TargetRoot { get; }
            public float Intensity { get; }
            public float ProximityScore { get; }
            public float FinalScore { get; }

            public BestNeedEvaluation(
                CustomerNeedState need,
                EntityRoot targetRoot,
                float intensity,
                float proximityScore,
                float finalScore)
            {
                Need = need;
                TargetRoot = targetRoot;
                Intensity = intensity;
                ProximityScore = proximityScore;
                FinalScore = finalScore;
            }
        }
    }
}