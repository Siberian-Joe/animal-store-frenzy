using System;
using Cysharp.Threading.Tasks;
using NewCore.Data;
using NewCore.Services;
using UnityEngine;

namespace NewCore.Commands
{
    public sealed class SpawnCustomerCommand : ICommand
    {
        public string Type { get; }
        public Vector3Int Position { get; }

        public SpawnCustomerCommand(string type, Vector3Int position)
        {
            Type = type;
            Position = position;
        }
    }

    public sealed class SpawnCustomerCommandHandler : ICommandHandler<SpawnCustomerCommand>
    {
        private readonly IGameDataService _gameDataService;

        public SpawnCustomerCommandHandler(IGameDataService gameDataService) => _gameDataService = gameDataService;

        public UniTask<bool> HandleAsync(SpawnCustomerCommand command)
        {
            if (!_gameDataService.TryRetrieveCachedData<GameState, GameStateProxy>(out var gameStateProxy))
                return UniTask.FromResult(false);

            var newCustomer = new Domain.Customer
            {
                Id = Guid.NewGuid().ToString(),
                Type = command.Type,
                Position = command.Position
            };

            gameStateProxy.Customers.AddModel(newCustomer);
            return UniTask.FromResult(true);
        }
    }
}