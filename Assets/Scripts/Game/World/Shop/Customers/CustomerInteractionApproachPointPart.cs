using System;
using Game.World.Features.InteractionTarget;
using Game.World.Interactions;
using UnityEngine;

namespace Game.World.Shop.Customers
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(InteractionTargetPart))]
    public sealed class CustomerInteractionApproachPointPart : InteractionApproachPointPart
    {
        public override bool Supports(IInteractionActor actor)
        {
            if (actor == null)
                throw new ArgumentNullException(nameof(actor));

            return actor.TryGetRole<ICustomerShoppingRole>(out _);
        }

        protected override Color GizmoColor => Color.yellow;
    }
}
