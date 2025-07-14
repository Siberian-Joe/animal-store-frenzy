using NewCore.Domain.UI;

namespace NewCore.Data.UI
{
    public class LoadingSystemOverlay : Proxy<LoadingSystemOverlayModel>
    {
        public override LoadingSystemOverlayModel ToModel() => new();
    }
}