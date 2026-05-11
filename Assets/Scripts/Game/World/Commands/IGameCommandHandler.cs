using System;

namespace Game.World.Commands
{
    public interface IGameCommandHandler<in TCommand> : IGameCommandHandler
        where TCommand : IGameCommand
    {
        void Execute(TCommand command);
    }

    public interface IGameCommandHandler
    {
        Type CommandType { get; }

        void Execute(IGameCommand command);
    }
}