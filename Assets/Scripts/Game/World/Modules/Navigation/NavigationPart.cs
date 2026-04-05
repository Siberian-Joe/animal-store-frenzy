using System;
using Game.World.Composition;
using R3;
using UnityEngine;
using UnityEngine.AI;

namespace Game.World.Navigation
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(NavMeshAgent))]
    public sealed class NavigationPart : FeaturePart<INavigationFeature>
    {
        [SerializeField] private float _stoppingDistance = 0.15f;
        [SerializeField] private bool _disableAgentRotation = true;
        [SerializeField] private bool _disableAgentUpAxis = true;

        private NavMeshAgent _agent;
        private IDisposable _arrivalSubscription;

        private int _bindingVersionCounter;
        private int _activeBindingVersion;

        public float StoppingDistance => _stoppingDistance;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();

            if (_disableAgentRotation)
                _agent.updateRotation = false;

            if (_disableAgentUpAxis)
                _agent.updateUpAxis = false;
        }

        public override void Bind(INavigationFeature feature, CompositeDisposable disposables)
        {
            if (feature == null)
                throw new ArgumentNullException(nameof(feature));

            if (disposables == null)
                throw new ArgumentNullException(nameof(disposables));

            var bindingVersion = ++_bindingVersionCounter;
            _activeBindingVersion = bindingVersion;

            StopPathInternal();

            _agent.stoppingDistance = feature.StoppingDistance;

            new BindingCleanup(bindingVersion, this)
                .AddTo(disposables);

            if (feature.HasTarget.Value)
                StartPath(feature, feature.TargetPosition.Value, bindingVersion);

            feature.TargetPosition
                .Skip(1)
                .Subscribe(targetPosition =>
                {
                    if (bindingVersion != _activeBindingVersion)
                        return;

                    if (feature.HasTarget.Value == false)
                        return;

                    StartPath(feature, targetPosition, bindingVersion);
                })
                .AddTo(disposables);

            feature.HasTarget
                .Skip(1)
                .Subscribe(hasTarget =>
                {
                    if (bindingVersion != _activeBindingVersion)
                        return;

                    if (hasTarget)
                    {
                        StartPath(feature, feature.TargetPosition.Value, bindingVersion);
                        return;
                    }

                    StopPath(bindingVersion);
                })
                .AddTo(disposables);
        }

        private void StartPath(
            INavigationFeature feature,
            Vector3 targetPosition,
            int bindingVersion)
        {
            if (bindingVersion != _activeBindingVersion)
                return;

            if (_agent == false || _agent.enabled == false || _agent.isOnNavMesh == false)
                return;

            DisposeArrivalSubscription();

            _agent.stoppingDistance = feature.StoppingDistance;
            _agent.SetDestination(targetPosition);

            _arrivalSubscription = _agent.WhenArrived()
                .Subscribe(_ =>
                {
                    if (bindingVersion != _activeBindingVersion)
                        return;

                    feature.NotifyArrived();
                });
        }

        private void StopPath(int bindingVersion)
        {
            if (bindingVersion != _activeBindingVersion)
                return;

            StopPathInternal();
        }

        private void StopPathInternal()
        {
            DisposeArrivalSubscription();

            if (_agent && _agent.enabled && _agent.isOnNavMesh)
                _agent.ResetPath();
        }

        private void DisposeArrivalSubscription()
        {
            _arrivalSubscription?.Dispose();
            _arrivalSubscription = null;
        }

        private void OnDestroy()
        {
            _activeBindingVersion = 0;
            StopPathInternal();
        }

        private sealed class BindingCleanup : IDisposable
        {
            private readonly int _bindingVersion;

            private NavigationPart _owner;

            public BindingCleanup(int bindingVersion, NavigationPart owner)
            {
                _bindingVersion = bindingVersion;
                _owner = owner;
            }

            public void Dispose()
            {
                if (_owner == false)
                    return;

                _owner.StopPath(_bindingVersion);
                _owner = null;
            }
        }
    }
}