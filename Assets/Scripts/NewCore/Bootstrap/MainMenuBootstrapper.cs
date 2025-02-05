using System;
using NewCore.Installers;
using NewCore.Services;
using NewCore.Services.UI;
using R3;
using UnityEngine;

namespace NewCore.Bootstrap
{
    public sealed class MainMenuBootstrapper : IBootstrapper
    {
        private readonly IUIRootLoader _uiRootLoader;
        private readonly ISceneLoader _sceneLoader;
        private readonly CompositeDisposable _disposables = new();

        public MainMenuBootstrapper(IUIRootLoader uiRootLoader, ISceneLoader sceneLoader)
        {
            _uiRootLoader = uiRootLoader;
            _sceneLoader = sceneLoader;
        }

        public async void Initialize()
        {
            try
            {
                var uiRoot = await _uiRootLoader.GetUIRootAsync();
                var mainMenu = uiRoot.EnableMainMenu();

                // TODO: This is only used to switch between scenes. Just a placeholder
                mainMenu.Clicked
                    .Subscribe(async _ => await _sceneLoader.LoadSceneAsync(SceneIdentifier.Core))
                    .AddTo(_disposables);
            }
            catch (Exception exception)
            {
                Debug.LogError("Failed to load main menu scene. Exception: " + exception);
            }
        }

        public void Dispose() => _disposables.Dispose();
    }
}