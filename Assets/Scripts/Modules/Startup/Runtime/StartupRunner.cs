using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Modules.Startup.Contracts;
using R3;
using Debug = UnityEngine.Debug;

namespace Modules.Startup.Runtime
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
            RunAsync(_token.Token).Forget(HandleUnhandledException);
        }

        private async UniTask RunAsync(CancellationToken token)
        {
            SetState(StartupState.Running);

            try
            {
                LastReport = await _startup.RunAsync(token);

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

        private void HandleUnhandledException(Exception exception)
        {
            if (_disposed)
                return;

            SetState(StartupState.Failed);
            Debug.LogException(exception);
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