using System;
using Game.Presentation.Runtime.Lifecycle;

namespace Game.Presentation.Runtime.Panels
{
    public interface IPanelPresenter
    {
        Type PanelType { get; }
    }

    public abstract class PanelPresenter : IPanelPresenter, IPanelPresenterLifecycle
    {
        public abstract Type PanelType { get; }

        void IPanelPresenterLifecycle.Attach(Panel panel) => AttachPanel(panel);
        void IPanelPresenterLifecycle.Opened() => OnOpened();
        void IPanelPresenterLifecycle.Closed() => OnClosed();
        void IPanelPresenterLifecycle.Released() => OnReleased();

        protected virtual void AttachPanel(Panel panel)
        {
            if (panel == false)
                throw new ArgumentNullException(nameof(panel));
        }

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

    public abstract class PanelPresenter<TPanel> : PanelPresenter
        where TPanel : Panel
    {
        private TPanel _panel;

        public sealed override Type PanelType => typeof(TPanel);

        protected TPanel Panel => _panel == false
            ? throw new InvalidOperationException($"{GetType().Name} has not been attached yet.")
            : _panel;

        protected sealed override void AttachPanel(Panel panel)
        {
            base.AttachPanel(panel);

            if (panel is not TPanel typedPanel)
            {
                throw new InvalidOperationException(
                    $"Presenter '{GetType().FullName}' expects panel '{typeof(TPanel).FullName}', " +
                    $"but received '{panel.GetType().FullName}'.");
            }

            if (_panel)
                throw new InvalidOperationException($"Presenter '{GetType().FullName}' is already attached.");

            _panel = typedPanel;
            OnPanelAttached(typedPanel);
        }

        protected virtual void OnPanelAttached(TPanel panel)
        {
        }
    }
}