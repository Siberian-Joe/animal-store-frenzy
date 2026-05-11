using System;
using System.Collections.Generic;

namespace Game.World.Commands
{
    public sealed class GameCommandDispatcher
    {
        private readonly Dictionary<Type, IGameCommandHandler> _handlers = new();
        private readonly IReadOnlyList<IGameCommandPostProcessor> _postProcessors;

        public GameCommandDispatcher(
            List<IGameCommandHandler> handlers,
            List<IGameCommandPostProcessor> postProcessors = null)
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

            _postProcessors = (IReadOnlyList<IGameCommandPostProcessor>)postProcessors ??
                              Array.Empty<IGameCommandPostProcessor>();
        }

        public void Dispatch(IGameCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var commandType = command.GetType();

            if (_handlers.TryGetValue(commandType, out var handler) == false)
                throw new InvalidOperationException($"No game command handler is registered for '{commandType.Name}'.");

            handler.Execute(command);

            foreach (var postProcessor in _postProcessors)
                postProcessor.Process(command);
        }
    }
}