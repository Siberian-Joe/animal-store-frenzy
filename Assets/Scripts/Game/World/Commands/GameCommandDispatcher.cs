using System;
using System.Collections.Generic;

namespace Game.World.Commands
{
    public interface IGameCommand
    {
    }

    public interface IGameCommandHandler
    {
        Type CommandType { get; }

        void Execute(IGameCommand command);
    }

    public interface IGameCommandHandler<in TCommand> : IGameCommandHandler
        where TCommand : IGameCommand
    {
        void Execute(TCommand command);
    }

    public abstract class GameCommandHandler<TCommand> : IGameCommandHandler<TCommand>
        where TCommand : IGameCommand
    {
        public Type CommandType => typeof(TCommand);

        public abstract void Execute(TCommand command);

        void IGameCommandHandler.Execute(IGameCommand command)
        {
            if (command is not TCommand typedCommand)
            {
                throw new InvalidOperationException(
                    $"Command handler '{GetType().Name}' cannot execute '{command?.GetType().Name ?? "<null>"}'.");
            }

            Execute(typedCommand);
        }
    }

    public sealed class GameCommandDispatcher
    {
        private readonly Dictionary<Type, IGameCommandHandler> _handlers = new();

        public GameCommandDispatcher(List<IGameCommandHandler> handlers)
        {
            if (handlers == null)
                throw new ArgumentNullException(nameof(handlers));

            foreach (var handler in handlers)
            {
                if (handler == null)
                    throw new InvalidOperationException("Command handler list contains a null entry.");

                if (_handlers.TryGetValue(handler.CommandType, out var existing))
                {
                    throw new InvalidOperationException(
                        $"Duplicate command handlers detected for '{handler.CommandType.Name}'. " +
                        $"Existing handler: '{existing.GetType().Name}', " +
                        $"new handler: '{handler.GetType().Name}'.");
                }

                _handlers.Add(handler.CommandType, handler);
            }
        }

        public void Dispatch(IGameCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var commandType = command.GetType();

            if (_handlers.TryGetValue(commandType, out var handler) == false)
            {
                throw new InvalidOperationException(
                    $"No game command handler is registered for '{commandType.Name}'.");
            }

            handler.Execute(command);
        }
    }
}