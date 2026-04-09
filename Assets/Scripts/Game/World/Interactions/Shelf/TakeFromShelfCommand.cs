using System;
using Game.World.Commands;
using Game.World.Interactions;

namespace Game.World.Interactions.Shelf
{
    public sealed class TakeFromShelfCommand : IGameCommand
    {
        public TakeFromShelfCommand(
            ITakeSourceContract source,
            ITakeReceiverContract receiver,
            int amount)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            Receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));
            Amount = amount > 0 ? amount : throw new ArgumentOutOfRangeException(nameof(amount));
        }

        public ITakeSourceContract Source { get; }

        public ITakeReceiverContract Receiver { get; }

        public int Amount { get; }
    }
}