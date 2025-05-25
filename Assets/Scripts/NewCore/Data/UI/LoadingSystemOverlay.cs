using NewCore.Domain.UI;

namespace NewCore.Data.UI
{
    public class LoadingSystemOverlay : Entity<LoadingSystemOverlayModel>
    {
        public override LoadingSystemOverlayModel ToModel() => new();
    }
}