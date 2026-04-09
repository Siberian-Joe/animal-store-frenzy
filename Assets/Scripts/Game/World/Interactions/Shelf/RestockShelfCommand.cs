using System;
using Game.World.Commands;
using Game.World.Interactions;

namespace Game.World.Interactions.Shelf
{
    public sealed class RestockShelfCommand : IGameCommand
    {
        public RestockShelfCommand(
            IRestockSourceContract source,
            IRestockTargetContract target,
            int amount)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Target = target ?? throw new ArgumentNullException(nameof(target));
            Amount = amount > 0 ? amount : throw new ArgumentOutOfRangeException(nameof(amount));
        }

        public IRestockSourceContract Source { get; }

        public IRestockTargetContract Target { get; }

        public int Amount { get; }
    }
}
