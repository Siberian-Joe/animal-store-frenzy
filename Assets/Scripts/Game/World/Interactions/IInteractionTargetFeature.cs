using Game.World.Core;
using UnityEngine;

namespace Game.World.Interactions
{
    public interface IInteractionTargetFeature : IEntityFeature
    {
        Vector3 ApproachPoint { get; }

        bool TryResolve(
            EntityRoot initiator,
            out IEntityInteraction interaction);
    }
}
