using NewCore.Data.UI;
using NewCore.Domain.UI;
using R3;

namespace NewCore.ViewModels.UI
{
    public class MainMenuScreenViewModel : ViewModel<MainMenuScreenModel, MainMenuScreenProxy>
    {
        public Observable<Unit> Clicked => _clicked;

        private readonly Subject<Unit> _clicked = new();

        public MainMenuScreenViewModel(MainMenuScreenProxy proxy) : base(proxy)
        {
        }

        public void OnClick() => _clicked.OnNext(Unit.Default);
    }
}