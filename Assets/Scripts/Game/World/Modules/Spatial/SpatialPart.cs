using Game.World.Composition;
using R3;
using UnityEngine;

namespace Game.World.Spatial
{
    [DisallowMultipleComponent]
    public class SpatialPart : FeaturePart<ISpatialFeature>
    {
        private const float PositionToleranceSqr = 0.0001f;
        private const float RotationToleranceDegrees = 0.1f;

        [SerializeField] private bool _syncPositionFromFeature = true;

        [SerializeField] private bool _syncRotationFromFeature = true;

        [SerializeField] private bool _syncPositionToFeature = true;

        [SerializeField] private bool _syncRotationToFeature = true;

        public override void Bind(ISpatialFeature feature, CompositeDisposable disposables)
        {
            if (_syncPositionFromFeature)
                transform.position = feature.Position.Value;

            if (_syncRotationFromFeature)
                transform.rotation = feature.Rotation.Value;

            if (_syncPositionFromFeature)
            {
                feature.Position
                    .Subscribe(position =>
                    {
                        if ((transform.position - position).sqrMagnitude <= PositionToleranceSqr)
                            return;

                        transform.position = position;
                    })
                    .AddTo(disposables);
            }

            if (_syncRotationFromFeature)
            {
                feature.Rotation
                    .Subscribe(rotation =>
                    {
                        if (Quaternion.Angle(transform.rotation, rotation) <= RotationToleranceDegrees)
                            return;

                        transform.rotation = rotation;
                    })
                    .AddTo(disposables);
            }

            if (_syncPositionToFeature || _syncRotationToFeature)
            {
                Observable.EveryUpdate()
                    .Subscribe(_ =>
                    {
                        if (_syncPositionToFeature && (feature.Position.Value - transform.position).sqrMagnitude >
                            PositionToleranceSqr)
                            feature.SetPosition(transform.position);

                        if (_syncRotationToFeature && Quaternion.Angle(feature.Rotation.Value, transform.rotation) >
                            RotationToleranceDegrees)
                            feature.SetRotation(transform.rotation);
                    })
                    .AddTo(disposables);
            }
        }
    }
}
