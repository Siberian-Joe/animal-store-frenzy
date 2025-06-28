using System;
using NewCore.Data;
using NewCore.Domain;
using NewCore.Extensions;
using NewCore.Services;

namespace NewCore.Commands
{
    public sealed class SpawnCustomerCommandHandler : ICommandHandler<SpawnCustomerCommand>
    {
        private readonly IGameDataResolver _gameDataResolver;

        public SpawnCustomerCommandHandler(IGameDataResolver gameDataResolver) => _gameDataResolver = gameDataResolver;

        public bool Handle(SpawnCustomerCommand command)
        {
            if (!_gameDataResolver.TryResolve<GameStateData, GameState>(out var gameStateProxy))
                return false;

            gameStateProxy.Customers.AddModel(new CustomerData
            {
                ID = Guid.NewGuid().ToString(),
                Type = command.Type,
                Position = command.Position
            });
            return true;
        }
    }
}