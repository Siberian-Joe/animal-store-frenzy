using System.Threading;
using Cysharp.Threading.Tasks;
using Modules.SceneReady.Contracts;

namespace Modules.SceneReady.Runtime
{
    public abstract class ReadyGate : IReadyGate
    {
        private readonly UniTaskCompletionSource _source = new();

        public bool IsReady { get; private set; }

        public UniTask WaitReadyAsync(CancellationToken token) =>
            IsReady
                ? UniTask.CompletedTask
                : _source.Task.AttachExternalCancellation(token);

        public void Open()
        {
            if (IsReady)
                return;

            IsReady = true;
            _source.TrySetResult();
        }
    }
}