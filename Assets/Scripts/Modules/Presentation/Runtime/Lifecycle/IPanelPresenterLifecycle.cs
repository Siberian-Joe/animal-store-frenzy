using Modules.Presentation.Runtime.Panels;

namespace Modules.Presentation.Runtime.Lifecycle
{
    public interface IPanelPresenterLifecycle
    {
        void Attach(Panel panel);
        void Opened();
        void Closed();
        void Released();
    }
}