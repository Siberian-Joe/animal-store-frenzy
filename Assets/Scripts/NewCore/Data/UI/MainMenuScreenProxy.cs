using NewCore.Domain.UI;

namespace NewCore.Data.UI
{
    public class MainMenuScreenProxy : EntityProxy<MainMenuScreenModel>
    {
        public override MainMenuScreenModel ToModel() => new();
    }
}