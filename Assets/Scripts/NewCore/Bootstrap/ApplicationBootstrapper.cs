using System;
using NewCore.Installers;
using NewCore.Services;
using NewCore.Services.UI;
using UnityEngine;

namespace NewCore.Bootstrap
{
    public sealed class ApplicationBootstrapper : IBootstrapper
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IUIRootLoader _uiRootLoader;

        public ApplicationBootstrapper(ISceneLoader sceneLoader, IUIRootLoader uiRootLoader)
        {
            _sceneLoader = sceneLoader;
            _uiRootLoader = uiRootLoader;
        }

        public async void Initialize()
        {
            try
            {
                var uiRoot = await _uiRootLoader.GetUIRootAsync();

                await _sceneLoader.LoadSceneAsync(SceneIdentifier.MainMenu);
                uiRoot.EnableMainMenu();
            }
            catch (Exception exception)
            {
                Debug.LogError("Failed to load main menu scene. Exception: " + exception);
            }
        }

        public void Dispose()
        {
        }
    }
}