using Game.Presentation.Runtime.Panels;

namespace Game.Presentation.Runtime.Lifecycle
{
    public interface IPanelPresenterLifecycle
    {
        void Attach(Panel panel);
        void Opened();
        void Closed();
        void Released();
    }
}