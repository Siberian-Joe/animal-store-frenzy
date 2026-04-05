using System;
using Game.World.Core;
using R3;
using UnityEngine;

namespace Game.World.Navigation
{
    public sealed class NavigationFeature : EntityFeature, INavigationFeature
    {
        private readonly NavigationState _state;
        private readonly float _stoppingDistance;
        private readonly Subject<Unit> _arrived = new();
        private readonly CompositeDisposable _disposables = new();

        public ReactiveProperty<bool> HasTarget { get; }
        public ReactiveProperty<Vector3> TargetPosition { get; }

        public Observable<Unit> Arrived => _arrived;

        public float StoppingDistance => _stoppingDistance;

        public NavigationFeature(
            NavigationState state,
            float stoppingDistance)
        {
            _state = state ?? throw new ArgumentNullException(nameof(state));
            _stoppingDistance = Mathf.Max(0f, stoppingDistance);

            HasTarget = new ReactiveProperty<bool>(_state.HasTarget);
            TargetPosition = new ReactiveProperty<Vector3>(_state.TargetPosition);

            HasTarget
                .Subscribe(value => _state.HasTarget = value)
                .AddTo(_disposables);

            TargetPosition
                .Subscribe(value => _state.TargetPosition = value)
                .AddTo(_disposables);
        }

        public void SetTarget(Vector3 targetPosition)
        {
            TargetPosition.Value = targetPosition;
            HasTarget.Value = true;
        }

        public void ClearTarget()
        {
            HasTarget.Value = false;
        }

        public void NotifyArrived()
        {
            if (HasTarget.Value == false)
                return;

            HasTarget.Value = false;
            _arrived.OnNext(Unit.Default);
        }

        public override void Dispose()
        {
            _disposables.Dispose();
            _arrived.Dispose();
            HasTarget.Dispose();
            TargetPosition.Dispose();

            base.Dispose();
        }
    }
}