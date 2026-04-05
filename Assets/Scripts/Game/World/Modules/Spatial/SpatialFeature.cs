using System;
using Game.World.Core;
using R3;
using UnityEngine;

namespace Game.World.Spatial
{
    public sealed class SpatialFeature : EntityFeature, ISpatialFeature
    {
        private readonly SpatialState _state;
        private readonly CompositeDisposable _disposables = new();

        public ReactiveProperty<Vector3> Position { get; }
        public ReactiveProperty<Quaternion> Rotation { get; }

        public SpatialFeature(SpatialState state)
        {
            _state = state ?? throw new ArgumentNullException(nameof(state));

            Position = new ReactiveProperty<Vector3>(_state.Position);
            Rotation = new ReactiveProperty<Quaternion>(_state.Rotation);

            Position
                .Subscribe(value => _state.Position = value)
                .AddTo(_disposables);

            Rotation
                .Subscribe(value => _state.Rotation = value)
                .AddTo(_disposables);
        }

        public void SetPosition(Vector3 position) => Position.Value = position;

        public void SetRotation(Quaternion rotation) => Rotation.Value = rotation;

        public void SetPose(Vector3 position, Quaternion rotation)
        {
            Position.Value = position;
            Rotation.Value = rotation;
        }

        public override void Dispose()
        {
            _disposables.Dispose();
            Position.Dispose();
            Rotation.Dispose();

            base.Dispose();
        }
    }
}
