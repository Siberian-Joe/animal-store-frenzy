using System.Threading;
using Cysharp.Threading.Tasks;

namespace Modules.Startup.Contracts
{
    public interface IStartupTask
    {
        string Name { get; }

        UniTask ExecuteAsync(CancellationToken token);
    }
}