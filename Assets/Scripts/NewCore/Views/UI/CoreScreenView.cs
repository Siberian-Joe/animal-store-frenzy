using NewCore.Attributes;
using NewCore.ViewModels.UI;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace NewCore.Views.UI
{
    [ResourceKey("CoreScreen")]
    public class CoreScreenView : ScreenView<CoreScreenViewModel>
    {
        [SerializeField] private Button _button;

        protected override void OnBind()
        {
            base.OnBind();
            _button.OnClickAsObservable()
                .Subscribe(_ => ViewModel.OnClick())
                .AddTo(this);
        }
    }
}