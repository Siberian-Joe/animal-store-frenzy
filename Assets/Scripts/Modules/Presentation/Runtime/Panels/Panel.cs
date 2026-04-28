using Modules.Presentation.Contracts;
using Modules.Presentation.Runtime.Lifecycle;
using UnityEngine;

namespace Modules.Presentation.Runtime.Panels
{
    public abstract class Panel : MonoBehaviour, IPanel, IPanelLifecycle
    {
        void IPanelLifecycle.Opened() => OnOpened();
        void IPanelLifecycle.Closed() => OnClosed();
        void IPanelLifecycle.Released() => OnReleased();

        protected virtual void OnOpened()
        {
        }

        protected virtual void OnClosed()
        {
        }

        protected virtual void OnReleased()
        {
        }
    }
}