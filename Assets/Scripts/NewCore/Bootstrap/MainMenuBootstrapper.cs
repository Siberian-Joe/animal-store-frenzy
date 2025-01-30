using NewCore.Services;
using NewCore.Views.UI;
using R3;

namespace NewCore.Bootstrap
{
    public sealed class MainMenuBootstrapper : IBootstrapper
    {
        private readonly UIRoot _uiRoot;
        private readonly ISceneLoader _sceneLoader;
        private readonly CompositeDisposable _disposables = new();

        public MainMenuBootstrapper(UIRoot uiRoot, ISceneLoader sceneLoader)
        {
            _uiRoot = uiRoot;
            _sceneLoader = sceneLoader;
        }

        public void Initialize()
        {
            var mainMenu = _uiRoot.EnableMainMenu();

            mainMenu.Clicked
                .Subscribe(async _ =>
                    await _sceneLoader.LoadSceneAsync(SceneIdentifier
                        .Core)) // TODO: English: This is only used to switch between scenes. Just a placeholder.
                .AddTo(_disposables);
        }

        public void Dispose() => _disposables.Dispose();
    }
}