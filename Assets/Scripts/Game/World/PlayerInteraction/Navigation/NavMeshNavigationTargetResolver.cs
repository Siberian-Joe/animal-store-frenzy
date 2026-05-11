using UnityEngine;
using UnityEngine.AI;

namespace Game.World.PlayerInteraction.Navigation
{
    public sealed class NavMeshNavigationTargetResolver : INavigationTargetResolver
    {
        private readonly float _sampleRadius;
        private readonly int _areaMask;

        public NavMeshNavigationTargetResolver(float sampleRadius, int areaMask)
        {
            _sampleRadius = Mathf.Max(0.01f, sampleRadius);
            _areaMask = areaMask;
        }

        public bool TryResolve(Vector3 requestedPosition, out Vector3 resolvedPosition)
        {
            if (NavMesh.SamplePosition(requestedPosition, out var hit, _sampleRadius, _areaMask) == false)
            {
                resolvedPosition = default;
                return false;
            }

            resolvedPosition = hit.position;
            return true;
        }
    }
}