using Game.Presentation.Runtime.Layers;
using Game.Presentation.Runtime.Panels;

namespace Game.Presentation.Runtime.Root
{
    public interface IPanelRoot
    {
        IPanelLayer GetLayerFor(Panel panel);
    }
}