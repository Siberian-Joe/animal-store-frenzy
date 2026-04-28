using Game.World.EntityRuntime;
using R3;
using UnityEngine;

namespace Game.World.Features.Spatial
{
    public interface ISpatialFeature : IEntityComponent
    {
        ReactiveProperty<Vector3> Position { get; }

        ReactiveProperty<Quaternion> Rotation { get; }

        void SetPosition(Vector3 position);

        void SetRotation(Quaternion rotation);

        void SetPose(Vector3 position, Quaternion rotation);
    }
}