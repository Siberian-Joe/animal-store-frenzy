using System.Threading;
using Cysharp.Threading.Tasks;

namespace Modules.SceneReady.Contracts
{
    public interface IReadyGate
    {
        bool IsReady { get; }

        UniTask WaitReadyAsync(CancellationToken token);
        void Open();
    }
}