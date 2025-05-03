using System;
using Cysharp.Threading.Tasks;
using NewCore.Data;
using NewCore.Domain;
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

        public async UniTask<bool> HandleAsync(SpawnCustomerCommand command)
        {
            var result = await _gameDataService.LoadAsync<GameState, GameStateProxy>();
            if (!result.IsSuccess)
                return await UniTask.FromResult(false);

            var gameStateProxy = result.Value;

            var newCustomer = new Customer
            {
                Id = Guid.NewGuid().ToString(),
                Type = command.Type,
                Position = command.Position
            };

            gameStateProxy.Customers.AddModel(newCustomer);
            return await UniTask.FromResult(true);
        }
    }
}