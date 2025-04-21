using NewCore.Data.UI;
using NewCore.Domain.UI;
using R3;

namespace NewCore.ViewModels.UI
{
    public class CoreScreenViewModel : ViewModel<CoreScreenModel, CoreScreenProxy>
    {
        public Observable<Unit> Clicked => _clicked;

        private readonly Subject<Unit> _clicked = new();

        public CoreScreenViewModel(CoreScreenProxy proxy) : base(proxy)
        {
        }

        public void OnClick() => _clicked.OnNext(Unit.Default);
    }
}