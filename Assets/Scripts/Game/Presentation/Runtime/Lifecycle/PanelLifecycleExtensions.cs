using System;
using Game.Presentation.Runtime.Panels;

namespace Game.Presentation.Runtime.Lifecycle
{
    public static class PanelLifecycleExtensions
    {
        public static void NotifyOpened(this Panel panel, PanelPresenter presenter)
        {
            if (panel == null)
                throw new ArgumentNullException(nameof(panel));
            if (presenter == null)
                throw new ArgumentNullException(nameof(presenter));

            ((IPanelLifecycle)panel).Opened();
            ((IPanelPresenterLifecycle)presenter).Opened();
        }

        public static void NotifyClosed(this Panel panel, PanelPresenter presenter)
        {
            if (panel == null)
                throw new ArgumentNullException(nameof(panel));
            if (presenter == null)
                throw new ArgumentNullException(nameof(presenter));

            ((IPanelPresenterLifecycle)presenter).Closed();
            ((IPanelLifecycle)panel).Closed();
        }

        public static void NotifyReleased(this Panel panel, PanelPresenter presenter)
        {
            if (panel == null)
                throw new ArgumentNullException(nameof(panel));
            if (presenter == null)
                throw new ArgumentNullException(nameof(presenter));

            ((IPanelPresenterLifecycle)presenter).Released();
            ((IPanelLifecycle)panel).Released();
        }
    }
}