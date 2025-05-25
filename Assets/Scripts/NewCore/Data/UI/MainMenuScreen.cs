using NewCore.Domain.UI;

namespace NewCore.Data.UI
{
    public class MainMenuScreen : Entity<MainMenuScreenModel>
    {
        public override MainMenuScreenModel ToModel() => new();
    }
}