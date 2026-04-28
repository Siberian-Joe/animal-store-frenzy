using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Modules.Presentation.Runtime.Preparation;
using Modules.Startup.Contracts;

namespace Game.Presentation.Startup
{
    [Startup(StartupPhase.Foundation, Order = -1000)]
    public sealed class PublishPresentationScopeStartupTask : IStartupTask, IDisposable
    {
        private readonly IPanelPreparationScope _scope;
        private readonly IPanelPreparationScopePublisher _publisher;

        private IDisposable _rollback;

        public string Name => "Publish presentation preparation scope";

        public PublishPresentationScopeStartupTask(
            IPanelPreparationScope scope,
            IPanelPreparationScopePublisher publisher)
        {
            _scope = scope ?? throw new ArgumentNullException(nameof(scope));
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
        }

        public UniTask ExecuteAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            if (_rollback != null)
                throw new InvalidOperationException(
                    $"{nameof(PublishPresentationScopeStartupTask)} is already executed.");

            _rollback = _publisher.Replace(_scope);
            return UniTask.CompletedTask;
        }

        public void Dispose()
        {
            _rollback?.Dispose();
            _rollback = null;
        }
    }
}