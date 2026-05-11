using System;
using Game.World.Commands;
using Game.World.EntityRuntime;
using Game.World.Inventory;
using Game.World.Persistence;
using Game.World.PlayerInteraction;

namespace Game.World.Pickups
{
    public sealed class PickupItemCommandHandler : GameCommandHandler<PickupItemCommand>
    {
        private readonly ILiveEntityRegistry _liveEntities;
        private readonly EntityActivator _entityActivator;
        private readonly IEntityFactory _entityFactory;
        private readonly IPlayerFeedback _feedback;

        public PickupItemCommandHandler(
            ILiveEntityRegistry liveEntities,
            EntityActivator entityActivator,
            IEntityFactory entityFactory,
            IPlayerFeedback feedback)
        {
            _liveEntities = liveEntities ?? throw new ArgumentNullException(nameof(liveEntities));
            _entityActivator = entityActivator ?? throw new ArgumentNullException(nameof(entityActivator));
            _entityFactory = entityFactory ?? throw new ArgumentNullException(nameof(entityFactory));
            _feedback = feedback ?? throw new ArgumentNullException(nameof(feedback));
        }

        public override void Execute(PickupItemCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (_liveEntities.TryGet(command.ActorId, out var actorRoot) == false || actorRoot == false)
                throw new InvalidOperationException(
                    $"Cannot pick up item because actor '{command.ActorId}' is not live.");

            if (_liveEntities.TryGet(command.PickupId, out var pickupRoot) == false || pickupRoot == false)
                throw new InvalidOperationException(
                    $"Cannot pick up item because pickup '{command.PickupId}' is not live.");

            var inventory = actorRoot.FindOwnedComponent<IInventory>();
            if (inventory == null)
                throw new InvalidOperationException($"Actor '{actorRoot.Id}' has no {nameof(IInventory)} component.");

            var pickup = pickupRoot.FindOwnedComponent<PickupPart>();
            if (pickup == false)
                throw new InvalidOperationException(
                    $"Pickup entity '{pickupRoot.Id}' has no {nameof(PickupPart)} component.");

            var expected = pickup.Contents;
            var requested = new ItemStack(new ItemId(command.ItemId), command.Amount);

            if (expected.ItemId != requested.ItemId || expected.Amount != requested.Amount)
            {
                throw new InvalidOperationException(
                    $"Pickup '{pickupRoot.Id}' contains '{expected}', but command requested '{requested}'.");
            }

            inventory.Add(requested);

            _entityActivator.RemoveState(pickupRoot);
            _entityActivator.Deactivate(pickupRoot);
            _entityFactory.Destroy(pickupRoot);

            _feedback.ShowMessage($"+{requested.Amount} {requested.ItemId}");
        }
    }
}