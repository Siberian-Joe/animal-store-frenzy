using System.Threading;
using Cysharp.Threading.Tasks;

namespace Modules.Readiness.Runtime.Contracts
{
    public interface IReadiness
    {
        bool IsReady { get; }

        UniTask WaitReadyAsync(CancellationToken token);
    }
}