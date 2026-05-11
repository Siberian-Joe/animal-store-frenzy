using System;
using System.Collections.Generic;
using Game.World.EntityRuntime;
using Game.World.Interactions;
using Game.World.Inventory;
using UnityEngine;

namespace Game.World.Upgrades
{
    public sealed class UpgradeInteractionCommandBuilderPart : InteractionCommandBuilderPart
    {
        [Header("Upgrade Interaction")] [SerializeField]
        private string _interactionActionId = "advance-upgrade-stage";

        [SerializeField] private UpgradeablePart _upgradeable;

        public override void CollectOptions(
            IInteractionActor actor,
            Vector3 approachPoint,
            List<InteractionOption> options)
        {
            if (actor == null)
                throw new ArgumentNullException(nameof(actor));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _upgradeable ??= GetComponent<UpgradeablePart>();
            if (_upgradeable == false || _upgradeable.HasNextStage == false)
                return;

            if (actor.TryGetRole<IInventoryRole>(out var inventoryRole) == false)
                return;

            var cost = _upgradeable.GetNextStageCost();
            if (inventoryRole.CanPay(cost) == false)
                return;

            if (actor is not Component actorComponent)
                return;

            var actorRoot = actorComponent.GetComponentInParent<EntityRoot>();
            var targetRoot = GetComponentInParent<EntityRoot>();

            if (actorRoot == false || targetRoot == false)
                return;

            var command = new AdvanceUpgradeStageCommand(actorRoot.Id, targetRoot.Id);

            options.Add(new InteractionOption(
                new InteractionActionId(_interactionActionId),
                approachPoint,
                command,
                targetRoot,
                _upgradeable.CurrentStageId.Value));
        }
    }
}