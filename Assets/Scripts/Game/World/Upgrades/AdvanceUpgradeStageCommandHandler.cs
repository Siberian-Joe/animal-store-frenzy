using System;
using Game.World.Commands;
using Game.World.Inventory;
using Game.World.Persistence;
using Game.World.PlayerInteraction;

namespace Game.World.Upgrades
{
    public sealed class AdvanceUpgradeStageCommandHandler : GameCommandHandler<AdvanceUpgradeStageCommand>
    {
        private readonly ILiveEntityRegistry _liveEntities;
        private readonly IPlayerFeedback _feedback;

        public AdvanceUpgradeStageCommandHandler(ILiveEntityRegistry liveEntities, IPlayerFeedback feedback)
        {
            _liveEntities = liveEntities ?? throw new ArgumentNullException(nameof(liveEntities));
            _feedback = feedback ?? throw new ArgumentNullException(nameof(feedback));
        }

        public override void Execute(AdvanceUpgradeStageCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (_liveEntities.TryGet(command.ActorId, out var actorRoot) == false || actorRoot == false)
                throw new InvalidOperationException($"Cannot upgrade because actor '{command.ActorId}' is not live.");

            if (_liveEntities.TryGet(command.UpgradeableId, out var targetRoot) == false || targetRoot == false)
                throw new InvalidOperationException(
                    $"Cannot upgrade because target '{command.UpgradeableId}' is not live.");

            var inventory = actorRoot.FindOwnedComponent<IInventory>();
            if (inventory == null)
                throw new InvalidOperationException($"Actor '{actorRoot.Id}' has no {nameof(IInventory)} component.");

            var upgradeable = targetRoot.FindOwnedComponent<UpgradeablePart>();
            if (upgradeable == false)
                throw new InvalidOperationException(
                    $"Target '{targetRoot.Id}' has no {nameof(UpgradeablePart)} component.");

            var beforeStage = upgradeable.CurrentStageId;
            var cost = upgradeable.GetNextStageCost();
            if (upgradeable.HasNextStage == false)
                throw new InvalidOperationException($"Target '{targetRoot.Id}' has no next upgrade stage.");

            if (inventory.TryRemove(cost) == false)
                throw new InvalidOperationException(
                    $"Actor '{actorRoot.Id}' cannot pay upgrade cost for '{targetRoot.Id}'.");

            upgradeable.Advance();
            _feedback.ShowMessage($"Upgraded: {beforeStage} -> {upgradeable.CurrentStageId}");
        }
    }
}