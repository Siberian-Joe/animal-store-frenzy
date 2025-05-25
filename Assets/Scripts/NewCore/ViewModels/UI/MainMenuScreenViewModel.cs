using NewCore.Data.UI;
using R3;

namespace NewCore.ViewModels.UI
{
    public class MainMenuScreenViewModel : ViewModel<MainMenuScreen>
    {
        public Observable<Unit> Clicked => _clicked;

        private readonly Subject<Unit> _clicked = new();

        public MainMenuScreenViewModel(MainMenuScreen proxy) : base(proxy)
        {
        }

        public void OnClick() => _clicked.OnNext(Unit.Default);
    }
}