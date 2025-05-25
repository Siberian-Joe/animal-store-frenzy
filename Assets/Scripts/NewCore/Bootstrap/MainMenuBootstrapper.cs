using System.Threading;
using Cysharp.Threading.Tasks;
using NewCore.Extensions;
using NewCore.Services.Scenes;
using NewCore.Services.UI;
using NewCore.ViewModels.UI;
using R3;
using MainMenuScreen = NewCore.Data.UI.MainMenuScreen;

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

        protected override async UniTask InitializeInternalAsync(CancellationToken cancellationToken = default)
        {
            var mainMenu = await _panelService
                .LoadPanelAsync<Views.UI.MainMenuScreenView, MainMenuScreen, MainMenuScreenViewModel>(cancellationToken)
                .AddTo(Disposables);

            mainMenu.Open();

            // TODO: This is only used to switch between scenes. Just a placeholder
            mainMenu.Context.Clicked
                .Subscribe(async _ => await _sceneLoader.LoadSceneAsync(SceneIdentifier.Core, cancellationToken))
                .AddTo(Disposables);

            await UniTask.Delay(1000, cancellationToken: cancellationToken);
        }
    }
}