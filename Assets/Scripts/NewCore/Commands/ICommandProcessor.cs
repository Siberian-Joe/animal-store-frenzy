using Cysharp.Threading.Tasks;

namespace NewCore.Commands
{
    public interface ICommandProcessor
    {
        void RegisterHandler<TCommand>(ICommandHandler<TCommand> handler) where TCommand : ICommand;
        UniTask<bool> TryProcessAsync<TCommand>(TCommand command) where TCommand : ICommand;
    }
}