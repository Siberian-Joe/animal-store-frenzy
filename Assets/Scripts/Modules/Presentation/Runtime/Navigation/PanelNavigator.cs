using System;
using Modules.Presentation.Runtime.Contracts.Navigation;
using Modules.Presentation.Runtime.Contracts.Registry;
using Modules.Presentation.Runtime.Panels;

namespace Modules.Presentation.Runtime.Navigation
{
    public sealed class PanelNavigator : IPanelNavigator
    {
        private readonly IPanelRegistry _registry;
        private readonly PanelNavigationHistory _history;

        public PanelNavigator(
            IPanelRegistry registry,
            PanelNavigationHistory history)
        {
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
            _history = history ?? throw new ArgumentNullException(nameof(history));
        }

        public bool CanGoBack => _history.CanGoBack;

        public void Open<TPresenter>()
            where TPresenter : PanelPresenter
        {
            var handle = _registry.Get<TPresenter>();

            _history.CloseAndRemoveDescendantsOf(handle.Layer);

            handle.Open();

            if (handle.Layer.RecordOpenedPanelsInNavigationHistory)
                _history.Push(handle);
        }

        public bool Back()
        {
            if (_history.CanGoBack == false)
                return false;

            if (_history.TryPopCurrent(out var current) == false)
                return false;

            current.Close();

            if (_history.TryGetCurrent(out var previous) == false)
                return false;

            previous.Open();

            return true;
        }

        public void Close<TPresenter>()
            where TPresenter : PanelPresenter
        {
            var handle = _registry.Get<TPresenter>();

            handle.Close();
            _history.Remove(handle);
        }

        public void Clear()
        {
            while (_history.TryPopCurrent(out var handle))
                handle.Close();

            _history.Clear();
        }
    }
}