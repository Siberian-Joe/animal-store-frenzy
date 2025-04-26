using Cysharp.Threading.Tasks;
using NewCore.Data.UI;
using NewCore.Extensions;
using NewCore.Services;
using NewCore.Services.UI;
using NewCore.ViewModels.UI;
using NewCore.Views.UI;
using R3;

namespace NewCore.Bootstrap
{
    public sealed class MainMenuBootstrapper : Bootstrapper
    {
        private readonly IPanelService _panelService;
        private readonly ISceneLoader _sceneLoader;

        public MainMenuBootstrapper(IPanelService panelService, ISceneLoader sceneLoader)
        {
            _panelService = panelService;
            _sceneLoader = sceneLoader;
        }

        protected override async UniTask InitializeInternalAsync()
        {
            var mainMenu = await _panelService
                .LoadPanelAsync<MainMenuScreen, MainMenuScreenProxy, MainMenuScreenViewModel>()
                .AddTo(Disposables);

            mainMenu.Open();

            // TODO: This is only used to switch between scenes. Just a placeholder
            mainMenu.Context.Clicked
                .Subscribe(async _ => await _sceneLoader.LoadSceneAsync(SceneIdentifier.Core))
                .AddTo(Disposables);

            await UniTask.Delay(1000);
        }
    }
}