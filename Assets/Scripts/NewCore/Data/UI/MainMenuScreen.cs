using NewCore.Domain.UI;

namespace NewCore.Data.UI
{
    public class MainMenuScreen : Proxy<MainMenuScreenModel>
    {
        public override MainMenuScreenModel ToModel() => new();
    }
}