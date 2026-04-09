using UnityEngine;

namespace Game.World.Interactions
{
    public interface IInteractionCommandBuilder
    {
        int Order { get; }

        bool TryBuild(
            IInteractionRoleResolver source,
            Vector3 approachPoint,
            out InteractionCommandRequest request);
    }
}