using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Startup.Contracts;
using R3;

namespace Game.Startup.Runtime
{
    public sealed class StartupRunner<TStartup> : IDisposable
        where TStartup : IStartup
    {
        public ReadOnlyReactiveProperty<StartupState> State => _state;
        public StartupRunReport LastReport { get; private set; }

        private readonly TStartup _startup;
        private readonly ReactiveProperty<StartupState> _state = new(StartupState.NotStarted);
        private readonly CancellationTokenSource _token = new();

        private bool _disposed;
        private bool _started;

        public StartupRunner(TStartup startup) =>
            _startup = startup ?? throw new ArgumentNullException(nameof(startup));

        public void Initialize()
        {
            if (_started || _disposed)
                return;

            _started = true;
            RunAsync().Forget();
        }

        private async UniTaskVoid RunAsync()
        {
            SetState(StartupState.Running);

            try
            {
                LastReport = await _startup.RunAsync(_token.Token);

                if (LastReport.HasCriticalFailure)
                {
                    SetState(StartupState.Failed);
                    return;
                }

                SetState(StartupState.Succeeded);
            }
            catch (OperationCanceledException)
            {
                SetState(StartupState.Cancelled);
            }
            catch
            {
                SetState(StartupState.Failed);
                throw;
            }
        }

        private void SetState(StartupState state)
        {
            if (_disposed)
                return;

            try
            {
                _state.Value = state;
            }
            catch (ObjectDisposedException)
            {
            }
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            try
            {
                _token.Cancel();
            }
            catch (ObjectDisposedException)
            {
            }

            _token.Dispose();
            _state.Dispose();
        }
    }
}