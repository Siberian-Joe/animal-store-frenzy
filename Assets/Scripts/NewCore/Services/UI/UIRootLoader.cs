using Cysharp.Threading.Tasks;
using NewCore.Services.Addressables;
using NewCore.Views.UI;
using UnityEngine;

namespace NewCore.Services.UI
{
    // TODO: A service that will be responsible for UI. Currently, it is a placeholder for successful resource loading
    public class UIRootLoader : IUIRootLoader
    {
        private readonly IAddressableResourceLoader _addressableResourceLoader;

        private UIRoot _uiRoot;
        private bool _isLoaded;

        public UIRootLoader(IAddressableResourceLoader addressableResourceLoader) =>
            _addressableResourceLoader = addressableResourceLoader;

        public async UniTask<UIRoot> GetUIRootAsync()
        {
            if (!_isLoaded)
            {
                _uiRoot = await _addressableResourceLoader.InstantiateResourceAsync<UIRoot>();

                Object.DontDestroyOnLoad(_uiRoot.gameObject);

                _isLoaded = true;
            }

            return _uiRoot;
        }
    }
}