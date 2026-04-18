using System;
using Game.World.EntityRuntime;
using Game.World.Features.InteractionTarget;
using Game.World.Shop.Exits;
using UnityEngine;

namespace Game.World.UtilityAi
{
    public readonly struct ExitOpportunityEntry
    {
        public StoreExitPointPart ExitPoint { get; }
        public EntityRoot Root { get; }
        public InteractionTargetPart InteractionTarget { get; }

        public bool HasInteractionTarget => InteractionTarget != false;

        public Vector3 ApproachPoint => HasInteractionTarget
            ? InteractionTarget.ApproachPoint
            : Root.transform.position;

        public ExitOpportunityEntry(
            StoreExitPointPart exitPoint,
            EntityRoot root,
            InteractionTargetPart interactionTarget)
        {
            ExitPoint = exitPoint ? exitPoint : throw new ArgumentNullException(nameof(exitPoint));
            Root = root ? root : throw new ArgumentNullException(nameof(root));
            InteractionTarget = interactionTarget;
        }
    }
}