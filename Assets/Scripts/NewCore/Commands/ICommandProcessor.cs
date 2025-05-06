namespace NewCore.Commands
{
    public interface ICommandProcessor
    {
        void RegisterHandler<TCommand>(ICommandHandler<TCommand> handler) where TCommand : ICommand;
        bool TryProcess<TCommand>(TCommand command) where TCommand : ICommand;
    }
}