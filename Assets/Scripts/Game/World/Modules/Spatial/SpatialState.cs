using System;
using Game.World.Core;
using UnityEngine;

namespace Game.World.Spatial
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