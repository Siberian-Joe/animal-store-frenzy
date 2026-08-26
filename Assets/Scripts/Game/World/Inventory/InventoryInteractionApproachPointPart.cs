using System;
using Game.World.Features.InteractionTarget;
using Game.World.Interactions;
using UnityEngine;

namespace Game.World.Inventory
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(InteractionTargetPart))]
    public sealed class InventoryInteractionApproachPointPart : InteractionApproachPointPart
    {
        public override bool Supports(IInteractionActor actor)
        {
            if (actor == null)
                throw new ArgumentNullException(nameof(actor));

            return actor.TryGetRole<IInventoryRole>(out _);
        }

        protected override Color GizmoColor => Color.cyan;
    }
}
