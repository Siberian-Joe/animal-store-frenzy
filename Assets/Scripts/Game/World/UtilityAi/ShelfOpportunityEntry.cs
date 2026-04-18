using System;
using Game.World.EntityRuntime;
using Game.World.Features.InteractionTarget;
using Game.World.Shop;
using Game.World.Shop.Shelves;
using UnityEngine;

namespace Game.World.UtilityAi
{
    public readonly struct ShelfOpportunityEntry
    {
        public ShelfProductPart Shelf { get; }
        public EntityRoot Root { get; }
        public InteractionTargetPart InteractionTarget { get; }
        public ProductId ProductId { get; }

        public bool HasInteractionTarget => InteractionTarget != false;

        public Vector3 ApproachPoint => HasInteractionTarget
            ? InteractionTarget.ApproachPoint
            : Root.transform.position;

        public ShelfOpportunityEntry(
            ShelfProductPart shelf,
            EntityRoot root,
            InteractionTargetPart interactionTarget)
        {
            Shelf = shelf ? shelf : throw new ArgumentNullException(nameof(shelf));
            Root = root ? root : throw new ArgumentNullException(nameof(root));
            InteractionTarget = interactionTarget;
            ProductId = shelf.ProductId;
        }
    }
}