using Modules.Presentation.Runtime.Layers;
using Modules.Presentation.Runtime.Panels;

namespace Modules.Presentation.Runtime.Root
{
    public interface IPresentationRoot
    {
        IPanelLayer GetLayerFor(Panel panel);
    }
}