using NewCore.Domain.UI;

namespace NewCore.Data.UI
{
    public class CoreScreen : Proxy<CoreScreenModel>
    {
        public override CoreScreenModel ToModel() => new();
    }
}