using UnityEngine;

namespace Game.World.PlayerInteraction.Navigation
{
    public interface INavigationTargetResolver
    {
        bool TryResolve(Vector3 requestedPosition, out Vector3 resolvedPosition);
    }
}