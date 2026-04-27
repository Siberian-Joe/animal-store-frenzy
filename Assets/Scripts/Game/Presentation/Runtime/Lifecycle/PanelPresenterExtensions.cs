using System;
using Game.Presentation.Runtime.Panels;

namespace Game.Presentation.Runtime.Lifecycle
{
    public static class PanelPresenterExtensions
    {
        public static void AttachTo(this PanelPresenter presenter, Panel panel)
        {
            if (presenter == null)
                throw new ArgumentNullException(nameof(presenter));
            if (panel == null)
                throw new ArgumentNullException(nameof(panel));

            ((IPanelPresenterLifecycle)presenter).Attach(panel);
        }
    }
}