using System;
using Game.World.Commands;
using Game.World.Inventory;
using Game.World.Persistence;
using Game.World.PlayerInteraction;

namespace Game.World.Processing
{
    public sealed class ProcessItemsCommandHandler : GameCommandHandler<ProcessItemsCommand>
    {
        private readonly ILiveEntityRegistry _liveEntities;
        private readonly IPlayerFeedback _feedback;

        public ProcessItemsCommandHandler(ILiveEntityRegistry liveEntities, IPlayerFeedback feedback)
        {
            _liveEntities = liveEntities ?? throw new ArgumentNullException(nameof(liveEntities));
            _feedback = feedback ?? throw new ArgumentNullException(nameof(feedback));
        }

        public override void Execute(ProcessItemsCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (_liveEntities.TryGet(command.ActorId, out var actorRoot) == false || actorRoot == false)
                throw new InvalidOperationException(
                    $"Cannot process items because actor '{command.ActorId}' is not live.");

            if (_liveEntities.TryGet(command.ProcessingPointId, out var processingRoot) == false ||
                processingRoot == false)
            {
                throw new InvalidOperationException(
                    $"Cannot process items because processing point '{command.ProcessingPointId}' is not live.");
            }

            var inventory = actorRoot.FindOwnedComponent<IInventory>();
            if (inventory == null)
                throw new InvalidOperationException($"Actor '{actorRoot.Id}' has no {nameof(IInventory)} component.");

            var processingPoint = processingRoot.FindOwnedComponent<ProcessingPointPart>();
            if (processingPoint == false)
            {
                throw new InvalidOperationException(
                    $"Processing entity '{processingRoot.Id}' has no {nameof(ProcessingPointPart)} component.");
            }

            var input = processingPoint.Input;
            var output = processingPoint.Output;
            if (inventory.TryRemove(input) == false)
            {
                throw new InvalidOperationException(
                    $"Actor '{actorRoot.Id}' cannot pay processing input for '{processingRoot.Id}'.");
            }

            inventory.Add(output);
            _feedback.ShowMessage($"-{input.Amount} {input.ItemId}, +{output.Amount} {output.ItemId}");
        }
    }
}
