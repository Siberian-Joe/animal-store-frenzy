using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NewCore.Data;
using NewCore.Services;
using UnityEngine;

namespace NewCore.Commands
{
    public sealed class CommandProcessor : ICommandProcessor
    {
        private readonly IGameDataService _gameDataService;
        private readonly Dictionary<Type, object> _handlers = new();

        public CommandProcessor(IGameDataService gameDataService) => _gameDataService = gameDataService;

        public void RegisterHandler<TCommand>(ICommandHandler<TCommand> handler) where TCommand : ICommand =>
            _handlers[typeof(TCommand)] = handler;

        public async UniTask<bool> TryProcessAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            if (_handlers.TryGetValue(typeof(TCommand), out var handler))
            {
                var typedHandler = (ICommandHandler<TCommand>)handler;
                var result = await typedHandler.HandleAsync(command);

                if (result)
                    await _gameDataService
                        .TrySaveAsync<GameState,
                            GameStateProxy>(); // TODO: Move save logic to a separate handler or external service to adhere to SRP.

                return result;
            }

            Debug.LogError($"No handler registered for command {typeof(TCommand).Name}");
            return false;
        }
    }
}