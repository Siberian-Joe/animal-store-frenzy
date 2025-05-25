using NewCore.Attributes;
using NewCore.ViewModels.UI;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace NewCore.Views.UI
{
    [ResourceKey("MainMenuScreen")]
    public class MainMenuScreenView : ScreenView<MainMenuScreenViewModel>
    {
        [SerializeField] private Button _button;

        protected override void OnBind()
        {
            _button.OnClickAsObservable()
                .Subscribe(_ => ViewModel.OnClick())
                .AddTo(this);
        }
    }
}