using NewCore.Domain.UI;

namespace NewCore.Data.UI
{
    public class CoreScreen : Proxy<CoreScreenModel>
    {
        public CoreScreen(CoreScreenModel model) : base(model)
        {
        }

        protected override CoreScreenModel CreateModel() => new();
    }
}