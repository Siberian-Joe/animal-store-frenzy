using System;
using System.Collections.Generic;
using Modules.Presentation.Runtime.Contracts;
using Modules.Presentation.Runtime.Contracts.Preparation;
using Modules.Presentation.Runtime.Panels;

namespace Modules.Presentation.Runtime.Registry
{
    public sealed class PanelRegistry : IPanelRegistryWriter
    {
        private readonly Dictionary<Type, IPanelLifetimeHandle> _handles = new();

        public IPanelHandle<TPresenter> Get<TPresenter>()
            where TPresenter : PanelPresenter
        {
            return TryGet<TPresenter>(out var handle)
                ? handle
                : throw new InvalidOperationException(
                    $"Panel '{typeof(TPresenter).FullName}' was not prepared. " +
                    $"Prepare it in startup via {nameof(IPanelPreparer)} before opening it.");
        }

        public bool TryGet<TPresenter>(out IPanelHandle<TPresenter> handle)
            where TPresenter : PanelPresenter
        {
            if (_handles.TryGetValue(typeof(TPresenter), out var existing) &&
                existing is IPanelHandle<TPresenter> typedHandle &&
                existing.IsReleased == false)
            {
                handle = typedHandle;
                return true;
            }

            handle = null;
            return false;
        }

        public void Register<TPresenter>(IPanelLifetimeHandle<TPresenter> handle)
            where TPresenter : PanelPresenter
        {
            if (handle == null)
                throw new ArgumentNullException(nameof(handle));

            var presenterType = typeof(TPresenter);

            if (_handles.TryAdd(presenterType, handle) == false)
            {
                throw new InvalidOperationException(
                    $"Panel handle for presenter '{presenterType.FullName}' is already registered.");
            }
        }

        public void Unregister(IPanelLifetimeHandle handle)
        {
            if (handle == null)
                return;

            if (_handles.TryGetValue(handle.PresenterType, out var existing) == false)
                return;

            if (ReferenceEquals(existing, handle))
                _handles.Remove(handle.PresenterType);
        }
    }
}