using System;

namespace Game.World.Commands
{
    public abstract class GameCommandHandler<TCommand> : IGameCommandHandler<TCommand>
        where TCommand : IGameCommand
    {
        public Type CommandType => typeof(TCommand);

        public abstract void Execute(TCommand command);

        void IGameCommandHandler.Execute(IGameCommand command)
        {
            if (command is not TCommand typedCommand)
                throw new InvalidOperationException(
                    $"Command handler '{GetType().Name}' cannot execute '{command?.GetType().Name ?? "<null>"}'.");

            Execute(typedCommand);
        }
    }
}