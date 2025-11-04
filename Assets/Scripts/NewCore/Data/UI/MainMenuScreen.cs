using NewCore.Domain.UI;

namespace NewCore.Data.UI
{
    public class MainMenuScreen : Proxy<MainMenuScreenModel>
    {
        public MainMenuScreen(MainMenuScreenModel model) : base(model)
        {
        }

        protected override MainMenuScreenModel CreateModel() => new();
    }
}