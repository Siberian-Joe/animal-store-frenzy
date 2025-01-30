using UnityEngine;

namespace NewCore.Views.UI
{
    public class UIRoot : MonoBehaviour
    {
        [SerializeField] private GameObject _loadingScreen;
        [SerializeField] private PanelBinder _mainMenu;
        [SerializeField] private PanelBinder _core;

        private void Awake()
        {
            HideLoadingScreen();

            _mainMenu.gameObject.SetActive(false);
            _core.gameObject.SetActive(false);
        }

        public void ShowLoadingScreen() => _loadingScreen.SetActive(true);

        public void HideLoadingScreen() => _loadingScreen.SetActive(false);

        public PanelBinder EnableMainMenu() => SwitchMenu(true);

        public PanelBinder EnableCore() => SwitchMenu(false);

        private PanelBinder SwitchMenu(bool isMainMenu)
        {
            _mainMenu.gameObject.SetActive(isMainMenu);
            _core.gameObject.SetActive(!isMainMenu);

            return isMainMenu ? _mainMenu : _core;
        }
    }
}