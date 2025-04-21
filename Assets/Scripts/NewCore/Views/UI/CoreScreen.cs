using NewCore.ViewModels.UI;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace NewCore.Views.UI
{
    public class CoreScreen : ScreenBinder<CoreScreenViewModel>
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