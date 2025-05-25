using NewCore.Data.UI;
using R3;

namespace NewCore.ViewModels.UI
{
    public class CoreScreenViewModel : ViewModel<CoreScreen>
    {
        public Observable<Unit> Clicked => _clicked;

        private readonly Subject<Unit> _clicked = new();

        public CoreScreenViewModel(CoreScreen proxy) : base(proxy)
        {
        }

        public void OnClick() => _clicked.OnNext(Unit.Default);
    }
}