using System;
using Game.World.EntityRuntime;
using UnityEngine;

namespace Game.World.Features.Spatial
{
    [Serializable]
    public sealed class SpatialState : IEntityStateData
    {
        public Vector3 Position;
        public Quaternion Rotation = Quaternion.identity;

        public IEntityStateData DeepClone()
        {
            return new SpatialState
            {
                Position = Position,
                Rotation = Rotation
            };
        }
    }
}