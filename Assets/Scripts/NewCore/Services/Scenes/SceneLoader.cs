using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using NewCore.Bootstrap;
using NewCore.Data.UI;
using NewCore.Services.UI;
using NewCore.Services.UI.Handlers;
using NewCore.ViewModels.UI;
using NewCore.Views.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using Zenject;
using Object = UnityEngine.Object;

namespace NewCore.Services.Scenes
{
    public sealed class SceneLoader : ISceneLoader, IDisposable
    {
        private readonly IPanelService _panelService;
        private AsyncOperationHandle<SceneInstance>? _activeSceneHandle;
        private IPanelHandler _loadingHandler;

        public SceneLoader(IPanelService panelService) => _panelService = panelService;

        public async UniTask LoadSceneAsync(string addressableKey, CancellationToken cancellationToken = default)
        {
            _loadingHandler = await LoadLoadingPanelAsync(cancellationToken);
            _loadingHandler.Open();

            try
            {
                var newSceneHandle = Addressables.LoadSceneAsync(addressableKey);
                await newSceneHandle.ToUniTask(cancellationToken: cancellationToken);

                ReleaseActiveScene();
                _activeSceneHandle = newSceneHandle;

                var sceneContext = Object.FindAnyObjectByType<SceneContext>();

                if (sceneContext == null)
                {
                    Debug.LogError($"SceneContext not found in loaded scene: {addressableKey}");
                    return;
                }

                await InitializeSceneBootstrapper(sceneContext.Container, cancellationToken);
            }
            finally
            {
                _loadingHandler.Close();
            }
        }

        private async UniTask<IPanelHandler> LoadLoadingPanelAsync(CancellationToken cancellationToken)
        {
            return await _panelService
                .LoadPanelAsync<
                    LoadingSystemOverlayView,
                    LoadingSystemOverlay,
                    LoadingSystemOverlayViewModel>(cancellationToken);
        }

        private static async UniTask InitializeSceneBootstrapper(DiContainer container,
            CancellationToken cancellationToken)
        {
            var bootstrapper = container.Resolve<IAsyncSceneBootstrapper>();
            await bootstrapper.InitializeAsync(cancellationToken);
        }

        private void ReleaseActiveScene()
        {
            if (!_activeSceneHandle.HasValue)
                return;

            if (_activeSceneHandle.Value.IsValid())
                Addressables.Release(_activeSceneHandle.Value);

            _activeSceneHandle = null;
        }

        public void Dispose() => ReleaseActiveScene();
    }
}