using System.Threading;
using Cysharp.Threading.Tasks;

namespace Modules.Startup.Runtime.Contracts
{
    public interface IStartup
    {
        UniTask<StartupRunReport> RunAsync(CancellationToken token);
    }
}