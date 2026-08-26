using System;
using Game.World.Features.Spatial;
using Game.World.Interactions;
using UnityEngine;

namespace Game.World.Features.InteractionTarget
{
    public abstract class InteractionApproachPointPart : MonoBehaviour
    {
        [SerializeField] private Vector3 _localOffset;

        public Vector3 LocalOffset => _localOffset;

        public abstract bool Supports(IInteractionActor actor);

        public Vector3 Resolve(SpatialPart spatial)
        {
            if (spatial == false)
                throw new ArgumentNullException(nameof(spatial));

            return spatial.Position.Value + spatial.Rotation.Value * _localOffset;
        }

        protected virtual Color GizmoColor => Color.white;

        protected virtual void OnDrawGizmosSelected()
        {
            var origin = transform.position;
            var point = origin + transform.rotation * _localOffset;

            Gizmos.color = GizmoColor;
            Gizmos.DrawLine(origin, point);
            Gizmos.DrawWireSphere(point, 0.12f);
        }
    }
}
