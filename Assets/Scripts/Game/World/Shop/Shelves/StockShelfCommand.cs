using System;
using Game.World.Commands;
using Game.World.EntityRuntime;

namespace Game.World.Shop.Shelves
{
    [Serializable]
    public sealed class StockShelfCommand : IGameCommand
    {
        public StockShelfCommand(
            EntityId actorId,
            EntityId shelfId,
            string itemId,
            int amount,
            string requiredStageId = null)
        {
            if (string.IsNullOrWhiteSpace(itemId))
                throw new ArgumentException("Item id cannot be null or whitespace.", nameof(itemId));

            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), amount, "Stock amount must be positive.");

            ActorId = actorId;
            ShelfId = shelfId;
            ItemId = itemId.Trim();
            Amount = amount;
            RequiredStageId = string.IsNullOrWhiteSpace(requiredStageId)
                ? null
                : requiredStageId.Trim();
        }

        public EntityId ActorId { get; }
        public EntityId ShelfId { get; }
        public string ItemId { get; }
        public int Amount { get; }
        public string RequiredStageId { get; }
    }
}