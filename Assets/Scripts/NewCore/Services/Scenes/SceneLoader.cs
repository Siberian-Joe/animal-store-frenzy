using System.Threading;
using Cysharp.Threading.Tasks;
using NewCore.Bootstrap;
using NewCore.Services.UI;
using NewCore.Services.UI.Handlers;
using NewCore.ViewModels.UI;
using NewCore.Views.UI;
using UnityEngine.SceneManagement;
using Zenject;
using LoadingSystemOverlay = NewCore.Data.UI.LoadingSystemOverlay;

namespace NewCore.Services.Scenes
{
    public class SceneLoader : ISceneLoader
    {
        private readonly ZenjectSceneLoader _zenjectLoader;
        private readonly IPanelService _panelService;

        private IPanelHandler _loadingHandler;

        public SceneLoader(ZenjectSceneLoader zenjectLoader, IPanelService panelService)
        {
            _zenjectLoader = zenjectLoader;
            _panelService = panelService;
        }

        public async UniTask LoadSceneAsync(string sceneName, CancellationToken cancellationToken = default)
        {
            _loadingHandler =
                await _panelService
                    .LoadPanelAsync<LoadingSystemOverlayView, LoadingSystemOverlay, LoadingSystemOverlayViewModel>(
                        cancellationToken);

            _loadingHandler.Open();

            DiContainer container = null;

            await _zenjectLoader
                .LoadSceneAsync(
                    sceneName,
                    LoadSceneMode.Single,
                    diContainer => container = diContainer)
                .WithCancellation(cancellationToken);

            var bootstrapper = container.Resolve<IAsyncSceneBootstrapper>();
            await bootstrapper.InitializeAsync(cancellationToken);

            _loadingHandler.Close();
        }

        public async UniTask UnloadSceneAsync(string sceneName, CancellationToken cancellationToken = default)
        {
            _loadingHandler.Open();

            var unloadOperation = SceneManager.UnloadSceneAsync(sceneName);
            await unloadOperation.ToUniTask(cancellationToken: cancellationToken);

            _loadingHandler.Close();
        }
    }
}