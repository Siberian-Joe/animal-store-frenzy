using System;
using Game.World.Core;
using UnityEngine;

namespace Game.World.Navigation
{
    [Serializable]
    public sealed class NavigationState : IEntityStateData
    {
        public bool HasTarget;
        public Vector3 TargetPosition;

        public IEntityStateData DeepClone()
        {
            return new NavigationState
            {
                HasTarget = HasTarget,
                TargetPosition = TargetPosition
            };
        }
    }
}