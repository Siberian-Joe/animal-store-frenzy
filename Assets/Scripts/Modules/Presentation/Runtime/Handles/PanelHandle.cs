using System;
using Modules.Presentation.Contracts;
using Modules.Presentation.Runtime.Layers;
using Modules.Presentation.Runtime.Lifecycle;
using Modules.Presentation.Runtime.Panels;
using Modules.Presentation.Runtime.Registry;
using Modules.ResourceLoading.Contracts;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Modules.Presentation.Runtime.Handles
{
    public abstract class PanelHandle : IPanelLifetimeHandle, IPanelRuntimeHandle
    {
        private readonly IPanelRegistryWriter _registry;
        private readonly IResourceLease<GameObject> _prefabLease;

        protected PanelHandle(
            Type presenterType,
            IPanelLayer layer,
            IPanelRegistryWriter registry,
            IResourceLease<GameObject> prefabLease,
            GameObject root,
            Panel panel,
            PanelPresenter presenter)
        {
            PresenterType = presenterType ?? throw new ArgumentNullException(nameof(presenterType));
            Layer = layer ?? throw new ArgumentNullException(nameof(layer));
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
            _prefabLease = prefabLease ?? throw new ArgumentNullException(nameof(prefabLease));

            Root = root ? root : throw new ArgumentNullException(nameof(root));
            PanelInstance = panel ? panel : throw new ArgumentNullException(nameof(panel));
            PresenterUntyped = presenter ?? throw new ArgumentNullException(nameof(presenter));
        }

        public IPanelLayer Layer { get; }

        public Type PresenterType { get; }

        public bool IsOpen { get; private set; }

        public bool IsReleased { get; private set; }

        protected GameObject Root { get; }

        protected Panel PanelInstance { get; }

        protected PanelPresenter PresenterUntyped { get; }

        public void Open()
        {
            ThrowIfReleased();
            Layer.Open(this);
        }

        public void Close()
        {
            ThrowIfReleased();
            Layer.Close(this);
        }

        public void Release()
        {
            if (IsReleased)
                return;

            Layer.Release(this);

            PanelInstance.NotifyReleased(PresenterUntyped);
            _registry.Unregister(this);

            Object.Destroy(Root);
            _prefabLease.Dispose();

            IsReleased = true;
        }

        void IPanelRuntimeHandle.OpenIn(Transform parent)
        {
            ThrowIfReleased();

            if (parent == false)
                throw new ArgumentNullException(nameof(parent));

            if (IsOpen)
                return;

            Root.transform.SetParent(parent, false);
            Root.SetActive(true);

            PanelInstance.NotifyOpened(PresenterUntyped);
            IsOpen = true;
        }

        void IPanelRuntimeHandle.CloseTo(Transform parent)
        {
            ThrowIfReleased();

            if (parent == false)
                throw new ArgumentNullException(nameof(parent));

            if (IsOpen == false)
            {
                Root.transform.SetParent(parent, false);
                Root.SetActive(false);
                return;
            }

            PanelInstance.NotifyClosed(PresenterUntyped);

            Root.SetActive(false);
            Root.transform.SetParent(parent, false);
            IsOpen = false;
        }

        private void ThrowIfReleased()
        {
            if (IsReleased)
                throw new InvalidOperationException($"Panel '{PresenterType.FullName}' is already released.");
        }
    }

    public sealed class PanelHandle<TPresenter> :
        PanelHandle,
        IPanelLifetimeHandle<TPresenter>
        where TPresenter : PanelPresenter
    {
        private readonly TPresenter _presenter;

        public PanelHandle(
            IPanelLayer layer,
            IPanelRegistryWriter registry,
            IResourceLease<GameObject> prefabLease,
            GameObject root,
            Panel panel,
            TPresenter presenter)
            : base(typeof(TPresenter), layer, registry, prefabLease, root, panel, presenter)
        {
            _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
        }

        public TPresenter Presenter => IsReleased
            ? throw new InvalidOperationException($"Panel '{typeof(TPresenter).FullName}' is already released.")
            : _presenter;
    }
}