using Game.World.Commands;

namespace Game.World.Interactions.Shelf
{
    public sealed class RestockShelfCommandHandler : GameCommandHandler<RestockShelfCommand>
    {
        public override void Execute(RestockShelfCommand command)
        {
            if (command.Source.AvailableQuantity <= 0 || command.Target.IsFull)
                return;

            var removed = command.Source.RemoveUpTo(command.Amount);
            if (removed <= 0)
                return;

            var accepted = command.Target.AddUpTo(removed);
            var remainder = removed - accepted;

            if (remainder > 0)
                command.Source.ReturnUpTo(remainder);
        }
    }
}