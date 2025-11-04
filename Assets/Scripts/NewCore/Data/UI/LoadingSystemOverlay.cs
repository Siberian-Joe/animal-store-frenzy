using NewCore.Domain.UI;

namespace NewCore.Data.UI
{
    public class LoadingSystemOverlay : Proxy<LoadingSystemOverlayModel>
    {
        public LoadingSystemOverlay(LoadingSystemOverlayModel model) : base(model)
        {
        }

        protected override LoadingSystemOverlayModel CreateModel() => new();
    }
}