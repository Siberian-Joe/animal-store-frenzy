using System;
using NewCore.Data;
using NewCore.Domain;
using NewCore.Services;

namespace NewCore.Commands
{
    public sealed class SpawnCustomerCommandHandler : ICommandHandler<SpawnCustomerCommand>
    {
        private readonly IGameDataResolver _gameDataResolver;

        public SpawnCustomerCommandHandler(IGameDataResolver gameDataResolver) => _gameDataResolver = gameDataResolver;

        public bool Handle(SpawnCustomerCommand command)
        {
            if (!_gameDataResolver.TryResolve<GameState, GameStateProxy>(out var gameStateProxy))
                return false;

            gameStateProxy.Customers.AddModel(new Customer
            {
                Id = Guid.NewGuid().ToString(),
                Type = command.Type,
                Position = command.Position
            });
            return true;
        }
    }
}