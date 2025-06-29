using NewCore.Data;
using NewCore.Services;

namespace NewCore.Commands
{
    public sealed class MovePlayerCommandHandler : ICommandHandler<MovePlayerCommand>
    {
        private readonly IGameDataResolver _gameDataResolver;

        public MovePlayerCommandHandler(IGameDataResolver gameDataResolver) => _gameDataResolver = gameDataResolver;

        public bool Handle(MovePlayerCommand command)
        {
            if (!_gameDataResolver.TryResolve<GameStateData, GameState>(out var gameState))
                return false;

            var player = gameState.Player.Value;

            if (gameState.Player == null)
                return false;

            player.TargetPosition.Value = command.TargetPosition;
            return true;
        }
    }
}