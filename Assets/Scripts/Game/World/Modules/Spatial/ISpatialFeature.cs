using R3;
using UnityEngine;

using Game.World.Core;

namespace Game.World.Spatial
{
    public interface ISpatialFeature : IEntityFeature
    {
        ReactiveProperty<Vector3> Position { get; }
        ReactiveProperty<Quaternion> Rotation { get; }

        void SetPosition(Vector3 position);
        void SetRotation(Quaternion rotation);
        void SetPose(Vector3 position, Quaternion rotation);
    }
}
