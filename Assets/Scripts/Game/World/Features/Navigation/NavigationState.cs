using System;
using Game.World.EntityRuntime;
using Game.World.Persistence;
using UnityEngine;

namespace Game.World.Features.Navigation
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
