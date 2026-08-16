using System;
using System.Collections.Generic;
using Game.World.EntityRuntime;
using Game.World.Interactions;
using Game.World.Inventory;
using UnityEngine;

namespace Game.World.Processing
{
    public sealed class ProcessingInteractionCommandBuilderPart : InteractionCommandBuilderPart
    {
        [Header("Processing Interaction")] [SerializeField]
        private string _interactionActionId = "process-items";

        [SerializeField] private ProcessingPointPart _processingPoint;

        public override void CollectOptions(
            IInteractionActor actor,
            Vector3 approachPoint,
            List<InteractionOption> options)
        {
            if (actor == null)
                throw new ArgumentNullException(nameof(actor));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _processingPoint ??= GetComponent<ProcessingPointPart>();
            if (_processingPoint == false)
                return;

            if (actor.TryGetRole<IInventoryRole>(out var inventoryRole) == false)
                return;

            var input = _processingPoint.Input;
            var output = _processingPoint.Output;
            if (inventoryRole.CanPay(new[] { input }) == false || inventoryRole.CanAccept(output) == false)
                return;

            if (actor is not Component actorComponent)
                return;

            var actorRoot = actorComponent.GetComponentInParent<EntityRoot>();
            var targetRoot = GetComponentInParent<EntityRoot>();
            if (actorRoot == false || targetRoot == false)
                return;

            options.Add(new InteractionOption(
                new InteractionActionId(_interactionActionId),
                approachPoint,
                new ProcessItemsCommand(actorRoot.Id, targetRoot.Id),
                targetRoot,
                output.ItemId.Value,
                output.Amount));
        }
    }
}
