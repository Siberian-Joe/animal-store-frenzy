using Game.World.Persistence;
using R3;
using UnityEngine;

namespace Game.World.Features.Spatial
{
    [DisallowMultipleComponent]
    public class SpatialPart : StatefulEntityComponent<SpatialState>, ISpatialFeature
    {
        private const float PositionToleranceSqr = 0.0001f;
        private const float RotationToleranceDegrees = 0.1f;

        private readonly ReactiveProperty<Vector3> _position = new(Vector3.zero);
        private readonly ReactiveProperty<Quaternion> _rotation = new(Quaternion.identity);

        public override int ActivationOrder => 100;

        public ReactiveProperty<Vector3> Position => _position;

        public ReactiveProperty<Quaternion> Rotation => _rotation;

        private void Awake()
        {
            _position.Value = transform.position;
            _rotation.Value = transform.rotation;
        }

        protected override void RestoreState(SpatialState state)
        {
        }

        protected override void InitializeFreshState(SpatialState state)
        {
            state.Position = transform.position;
            state.Rotation = transform.rotation;
        }

        protected override void ApplyBoundState(SpatialState state)
        {
            _position.Value = state.Position;
            _rotation.Value = state.Rotation;
            ApplyTransform(state.Position, state.Rotation);
        }

        protected override void OnStateActivated()
        {
            _position
                .Subscribe(ApplyPosition)
                .AddTo(ActivationDisposables);

            _rotation
                .Subscribe(ApplyRotation)
                .AddTo(ActivationDisposables);

            Observable.EveryUpdate()
                .Subscribe(_ => ReconcileFromTransform())
                .AddTo(ActivationDisposables);
        }

        public void SetPosition(Vector3 position) => _position.Value = position;

        public void SetRotation(Quaternion rotation) => _rotation.Value = rotation;

        public void SetPose(Vector3 position, Quaternion rotation)
        {
            _position.Value = position;
            _rotation.Value = rotation;
        }

        protected override void OnDestroy()
        {
            _position.Dispose();
            _rotation.Dispose();
            base.OnDestroy();
        }

        private void ApplyPosition(Vector3 position)
        {
            State.Position = position;

            if ((transform.position - position).sqrMagnitude <= PositionToleranceSqr)
                return;

            transform.position = position;
        }

        private void ApplyRotation(Quaternion rotation)
        {
            State.Rotation = rotation;

            if (Quaternion.Angle(transform.rotation, rotation) <= RotationToleranceDegrees)
                return;

            transform.rotation = rotation;
        }

        private void ApplyTransform(Vector3 position, Quaternion rotation)
        {
            transform.SetPositionAndRotation(position, rotation);
        }

        private void ReconcileFromTransform()
        {
            if ((_position.Value - transform.position).sqrMagnitude > PositionToleranceSqr)
                _position.Value = transform.position;

            if (Quaternion.Angle(_rotation.Value, transform.rotation) > RotationToleranceDegrees)
                _rotation.Value = transform.rotation;
        }
    }
}