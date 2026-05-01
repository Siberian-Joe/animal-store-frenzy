using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Modules.Readiness.Runtime.Contracts;
using Modules.SceneNavigation.Runtime.Contracts;
using UnityEngine;

namespace Modules.SceneNavigation.Runtime.Navigators
{
    public sealed class SceneNavigator : ISceneNavigator, IDisposable
    {
        private readonly ISceneCatalog _catalog;
        private readonly ISceneLoader _sceneLoader;
        private readonly ISceneReadinessAwaiter _readinessAwaiter;
        private readonly ISceneTransitionScreen _transitionScreen;
        private readonly IScenePresentationCleaner _presentationCleaner;
        private readonly CancellationTokenSource _lifetimeToken = new();

        private bool _disposed;

        public bool IsTransitionRunning { get; private set; }

        public SceneNavigator(
            ISceneCatalog catalog,
            ISceneLoader sceneLoader,
            ISceneReadinessAwaiter readinessAwaiter,
            ISceneTransitionScreen transitionScreen,
            IScenePresentationCleaner presentationCleaner)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
            _readinessAwaiter = readinessAwaiter ?? throw new ArgumentNullException(nameof(readinessAwaiter));
            _transitionScreen = transitionScreen ?? throw new ArgumentNullException(nameof(transitionScreen));
            _presentationCleaner = presentationCleaner ?? throw new ArgumentNullException(nameof(presentationCleaner));
        }

        public bool TryGoTo<TRoute>()
            where TRoute : SceneRoute
        {
            if (_disposed)
                return false;

            if (IsTransitionRunning)
                return false;

            RunTransitionAsync(typeof(TRoute), _lifetimeToken.Token)
                .Forget(Debug.LogException);

            return true;
        }

        private async UniTask RunTransitionAsync(
            Type targetRouteType,
            CancellationToken token)
        {
            IsTransitionRunning = true;

            var transitionShown = false;

            try
            {
                await UniTask.SwitchToMainThread(token);
                await _transitionScreen.ShowAsync(token);

                transitionShown = true;

                _presentationCleaner.ClearScenePresentation();

                await _sceneLoader.LoadSingleAsync(_catalog.TempSceneReference, token);

                var targetReference = _catalog.GetReference(targetRouteType);
                var targetScene = await _sceneLoader.LoadSingleAsync(
                    targetReference,
                    token);

                await _readinessAwaiter.WaitReadyAsync(targetScene, token);
                await UniTask.NextFrame(token);
            }
            finally
            {
                await UniTask.SwitchToMainThread();

                try
                {
                    if (_disposed == false && transitionShown)
                        await _transitionScreen.HideAsync(CancellationToken.None);
                }
                finally
                {
                    IsTransitionRunning = false;
                }
            }
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            _lifetimeToken.Cancel();
            _lifetimeToken.Dispose();
        }
    }
}