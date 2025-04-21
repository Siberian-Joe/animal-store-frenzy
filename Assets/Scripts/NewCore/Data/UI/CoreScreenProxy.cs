using NewCore.Domain.UI;

namespace NewCore.Data.UI
{
    public class CoreScreenProxy : EntityProxy<CoreScreenModel>
    {
        public override CoreScreenModel ToModel() => new();
    }
}