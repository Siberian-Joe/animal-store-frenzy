using Game.World.Commands;

namespace Game.World.Interactions.Shelf
{
    public sealed class TakeFromShelfCommandHandler : GameCommandHandler<TakeFromShelfCommand>
    {
        public override void Execute(TakeFromShelfCommand command)
        {
            if (command.Source.IsEmpty || command.Receiver.IsFull)
                return;

            var removed = command.Source.RemoveUpTo(command.Amount);
            if (removed <= 0)
                return;

            var accepted = command.Receiver.AddUpTo(removed);
            var remainder = removed - accepted;

            if (remainder > 0)
                command.Source.ReturnUpTo(remainder);
        }
    }
}