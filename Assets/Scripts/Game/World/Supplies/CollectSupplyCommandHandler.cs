using System;
using Game.World.Commands;
using Game.World.Inventory;
using Game.World.Persistence;
using Game.World.PlayerInteraction;

namespace Game.World.Supplies
{
    public sealed class CollectSupplyCommandHandler : GameCommandHandler<CollectSupplyCommand>
    {
        private readonly ILiveEntityRegistry _liveEntities;
        private readonly IPlayerFeedback _feedback;

        public CollectSupplyCommandHandler(
            ILiveEntityRegistry liveEntities,
            IPlayerFeedback feedback)
        {
            _liveEntities = liveEntities ?? throw new ArgumentNullException(nameof(liveEntities));
            _feedback = feedback ?? throw new ArgumentNullException(nameof(feedback));
        }

        public override void Execute(CollectSupplyCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (_liveEntities.TryGet(command.ActorId, out var actorRoot) == false || actorRoot == false)
            {
                throw new InvalidOperationException(
                    $"Cannot collect supply because actor '{command.ActorId}' is not live.");
            }

            if (_liveEntities.TryGet(command.SupplyPointId, out var supplyRoot) == false || supplyRoot == false)
            {
                throw new InvalidOperationException(
                    $"Cannot collect supply because supply point '{command.SupplyPointId}' is not live.");
            }

            var inventory = actorRoot.FindOwnedComponent<IInventory>();
            if (inventory == null)
                throw new InvalidOperationException($"Actor '{actorRoot.Id}' has no {nameof(IInventory)} component.");

            var supplyPoint = supplyRoot.FindOwnedComponent<ItemSupplyPointPart>();
            if (supplyPoint == false)
            {
                throw new InvalidOperationException(
                    $"Supply entity '{supplyRoot.Id}' has no {nameof(ItemSupplyPointPart)} component.");
            }

            var contents = supplyPoint.Contents;
            inventory.Add(contents);
            _feedback.ShowMessage($"+{contents.Amount} {contents.ItemId}");
        }
    }
}