using System;
using Game.World.EntityRuntime;
using R3;
using UnityEngine;
using UnityEngine.AI;

namespace Game.World.Features.Navigation
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(NavMeshAgent))]
    public sealed class NavigationPart : StatefulEntityComponent<NavigationState>, INavigationFeature
    {
        [SerializeField] private float _stoppingDistance = 0.15f;
        [SerializeField] private bool _disableAgentRotation = true;
        [SerializeField] private bool _disableAgentUpAxis = true;

        private readonly Subject<Unit> _arrived = new();
        private readonly ReactiveProperty<bool> _hasTarget = new(false);
        private readonly ReactiveProperty<Vector3> _targetPosition = new(Vector3.zero);

        private NavMeshAgent _agent;
        private IDisposable _arrivalSubscription;

        private int _activationVersionCounter;
        private int _activeActivationVersion;

        public override int ActivationOrder => 200;

        public ReactiveProperty<bool> HasTarget => _hasTarget;

        public ReactiveProperty<Vector3> TargetPosition => _targetPosition;

        public Observable<Unit> Arrived => _arrived;

        public float StoppingDistance => Mathf.Max(0f, _stoppingDistance);

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();

            if (_disableAgentRotation)
                _agent.updateRotation = false;

            if (_disableAgentUpAxis)
                _agent.updateUpAxis = false;

            _targetPosition.Value = transform.position;
        }

        protected override void RestoreState(NavigationState state)
        {
        }

        protected override void InitializeFreshState(NavigationState state)
        {
            state.HasTarget = false;
            state.TargetPosition = transform.position;
        }

        protected override void OnStateReady(NavigationState state)
        {
            _targetPosition.Value = state.TargetPosition;
            _hasTarget.Value = state.HasTarget;
        }

        protected override void OnActivate()
        {
            _hasTarget
                .Subscribe(value => State.HasTarget = value)
                .AddTo(ActivationDisposables);

            _targetPosition
                .Subscribe(value => State.TargetPosition = value)
                .AddTo(ActivationDisposables);

            var activationVersion = ++_activationVersionCounter;
            _activeActivationVersion = activationVersion;

            StopPathInternal();

            _agent.stoppingDistance = StoppingDistance;

            new ActivationCleanup(activationVersion, this)
                .AddTo(ActivationDisposables);

            if (_hasTarget.Value)
                StartPath(_targetPosition.Value, activationVersion);

            _targetPosition
                .Skip(1)
                .Subscribe(targetPosition =>
                {
                    if (activationVersion != _activeActivationVersion)
                        return;

                    if (_hasTarget.Value == false)
                        return;

                    StartPath(targetPosition, activationVersion);
                })
                .AddTo(ActivationDisposables);

            _hasTarget
                .Skip(1)
                .Subscribe(hasTarget =>
                {
                    if (activationVersion != _activeActivationVersion)
                        return;

                    if (hasTarget)
                    {
                        StartPath(_targetPosition.Value, activationVersion);
                        return;
                    }

                    StopPath(activationVersion);
                })
                .AddTo(ActivationDisposables);
        }

        protected override void OnDeactivate()
        {
            _activeActivationVersion = 0;
            StopPathInternal();
        }

        public void SetTarget(Vector3 targetPosition)
        {
            _targetPosition.Value = targetPosition;
            _hasTarget.Value = true;
        }

        public void ClearTarget()
        {
            _hasTarget.Value = false;
        }

        public void NotifyArrived()
        {
            if (_hasTarget.Value == false)
                return;

            _hasTarget.Value = false;
            _arrived.OnNext(Unit.Default);
        }

        private void StartPath(Vector3 targetPosition, int activationVersion)
        {
            if (activationVersion != _activeActivationVersion)
                return;

            if (_agent == false || _agent.enabled == false || _agent.isOnNavMesh == false)
                return;

            DisposeArrivalSubscription();

            _agent.stoppingDistance = StoppingDistance;
            _agent.SetDestination(targetPosition);

            _arrivalSubscription = _agent.WhenArrived()
                .Subscribe(_ =>
                {
                    if (activationVersion != _activeActivationVersion)
                        return;

                    NotifyArrived();
                });
        }

        private void StopPath(int activationVersion)
        {
            if (activationVersion != _activeActivationVersion)
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

        protected override void OnDestroy()
        {
            _activeActivationVersion = 0;
            StopPathInternal();
            _arrived.Dispose();
            _hasTarget.Dispose();
            _targetPosition.Dispose();
            base.OnDestroy();
        }

        private sealed class ActivationCleanup : IDisposable
        {
            private readonly int _activationVersion;
            private NavigationPart _owner;

            public ActivationCleanup(int activationVersion, NavigationPart owner)
            {
                _activationVersion = activationVersion;
                _owner = owner;
            }

            public void Dispose()
            {
                if (_owner == false)
                    return;

                _owner.StopPath(_activationVersion);
                _owner = null;
            }
        }
    }
}
