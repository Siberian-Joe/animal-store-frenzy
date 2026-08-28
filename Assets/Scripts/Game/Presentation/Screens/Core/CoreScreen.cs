using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Screen = Modules.Presentation.Runtime.Panels.Screen;

namespace Game.Presentation.Screens.Core
{
    public sealed class CoreScreen : Screen
    {
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private TMP_Text _gameTimeText;
        [SerializeField] private TMP_Text _storeStatusText;
        [SerializeField] private TMP_Text _objectiveText;
        [SerializeField] private TMP_Text _inventoryText;
        [SerializeField] private TMP_Text _feedbackText;

        public Observable<Unit> MainMenuRequested => _mainMenuButton.onClick.AsObservable();

        public void SetGameTimeText(string text)
        {
            if (_gameTimeText)
                _gameTimeText.text = text ?? string.Empty;
        }

        public void SetStoreStatusText(string text)
        {
            if (_storeStatusText)
                _storeStatusText.text = text ?? string.Empty;
        }

        public void SetObjectiveText(string text)
        {
            if (_objectiveText)
                _objectiveText.text = text ?? string.Empty;
        }

        public void SetInventoryText(string text)
        {
            if (_inventoryText)
                _inventoryText.text = text ?? string.Empty;
        }

        public void SetFeedbackText(string text)
        {
            if (_feedbackText)
                _feedbackText.text = text ?? string.Empty;
        }
    }
}
