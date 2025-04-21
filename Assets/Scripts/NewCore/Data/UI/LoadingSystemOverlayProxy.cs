using NewCore.Domain.UI;

namespace NewCore.Data.UI
{
    public class LoadingSystemOverlayProxy : EntityProxy<LoadingSystemOverlayModel>
    {
        public override LoadingSystemOverlayModel ToModel() => new();
    }
}