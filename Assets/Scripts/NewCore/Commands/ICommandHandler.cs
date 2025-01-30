using Cysharp.Threading.Tasks;

namespace NewCore.Commands
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        UniTask<bool> HandleAsync(TCommand command);
    }
}