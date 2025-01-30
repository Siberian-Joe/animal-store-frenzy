using System;
using NewCore.Services;
using NewCore.Views.UI;
using UnityEngine;

namespace NewCore.Bootstrap
{
    public sealed class ApplicationBootstrapper : IBootstrapper
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly UIRoot _uiRoot;

        public ApplicationBootstrapper(ISceneLoader sceneLoader, UIRoot uiRoot)
        {
            _sceneLoader = sceneLoader;
            _uiRoot = uiRoot;
        }

        public async void Initialize()
        {
            try
            {
                await _sceneLoader.LoadSceneAsync(SceneIdentifier.MainMenu);
                _uiRoot.EnableMainMenu();
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