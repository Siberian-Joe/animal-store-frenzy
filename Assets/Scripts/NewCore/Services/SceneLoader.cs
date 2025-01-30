using Cysharp.Threading.Tasks;
using NewCore.Views.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NewCore.Services
{
    public class SceneLoader : ISceneLoader
    {
        private readonly UIRoot _uiRoot;
        
        public SceneLoader(UIRoot uiRoot) => _uiRoot = uiRoot;

        public async UniTask LoadSceneAsync(string sceneName)
        {
            _uiRoot.ShowLoadingScreen();

            var loadOperation = SceneManager.LoadSceneAsync(sceneName);
            await loadOperation.ToUniTask();

            var scene = SceneManager.GetSceneByName(sceneName);
            if (!scene.isLoaded)
            {
                Debug.LogError($"Scene {sceneName} failed to load.");
                return;
            }

            _uiRoot.HideLoadingScreen();
        }

        public async UniTask UnloadSceneAsync(string sceneName)
        {
            _uiRoot.ShowLoadingScreen();

            var unloadOperation = SceneManager.UnloadSceneAsync(sceneName);
            await unloadOperation.ToUniTask();

            _uiRoot.HideLoadingScreen();
        }
    }
}