using Cysharp.Threading.Tasks;
using NewCore.Bootstrap;
using NewCore.Data.UI;
using NewCore.Installers;
using NewCore.Services.UI;
using NewCore.Services.UI.Handlers;
using NewCore.ViewModels.UI;
using NewCore.Views.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace NewCore.Services
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

        public async UniTask LoadSceneAsync(string sceneName)
        {
            _loadingHandler = await _panelService
                .LoadPanelAsync<LoadingSystemOverlay, LoadingSystemOverlayProxy, LoadingSystemOverlayViewModel>();

            _loadingHandler.Open();

            DiContainer container = null;

            await _zenjectLoader.LoadSceneAsync(
                sceneName,
                LoadSceneMode.Single,
                diContainer => container = diContainer);

            var bootstrapper = container.Resolve<IAsyncSceneBootstrapper>();
            await bootstrapper.InitializeAsync();

            _loadingHandler.Close();
        }

        public async UniTask UnloadSceneAsync(string sceneName)
        {
            _loadingHandler.Open();

            var unloadOperation = SceneManager.UnloadSceneAsync(sceneName);
            await unloadOperation.ToUniTask();

            _loadingHandler.Close();
        }
    }
}