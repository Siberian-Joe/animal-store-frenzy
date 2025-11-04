using System;
using NewCore.Data;
using NewCore.Domain;
using NewCore.Extensions;
using NewCore.Modules.Interaction;
using NewCore.Services;
using NewCore.Services.GameData;

namespace NewCore.Commands
{
    public sealed class SpawnCustomerCommandHandler : ICommandHandler<SpawnCustomerCommand>
    {
        private readonly IGameDataResolver _gameDataResolver;
        private readonly IProxyFactory _proxyFactory;

        public SpawnCustomerCommandHandler(IGameDataResolver gameDataResolver, IProxyFactory proxyFactory)
        {
            _gameDataResolver = gameDataResolver;
            _proxyFactory = proxyFactory;
        }

        public bool Handle(SpawnCustomerCommand command)
        {
            if (_gameDataResolver.TryResolve<GameStateData, GameState>(out var gameStateProxy) == false)
                return false;

            gameStateProxy.Customers.AddModel(new CustomerData
            {
                Id = Guid.NewGuid().ToString(),
                Type = command.Type,
                Position = command.Position
            }, _proxyFactory);

            return true;
        }
    }
}