using Modules.Presentation.Runtime.Layers;
using Modules.Presentation.Runtime.Panels;

namespace Modules.Presentation.Runtime.Root
{
    public interface IPanelRoot
    {
        IPanelLayer GetLayerFor(Panel panel);
    }
}