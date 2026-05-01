using System;
using Modules.Presentation.Contracts;
using Modules.Presentation.Runtime.Handles;
using Modules.Presentation.Runtime.Panels;
using Modules.Presentation.Runtime.Root;
using UnityEngine;

namespace Modules.Presentation.Runtime.Layers
{
    public abstract class PanelLayer : MonoBehaviour, IPanelLayer
    {
        [Header("Roots")]
        [Tooltip("Required. Opened panel instances handled by this layer are moved here.")]
        [SerializeField]
        private Transform _contentRoot;

        [Tooltip("Required. Closed prepared panel instances handled by this layer are moved here.")] [SerializeField]
        private Transform _cacheRoot;

        [Header("Navigation")]
        [Tooltip(
            "If enabled, panels opened by this layer are recorded in panel navigation history and can be closed by Back(). Disable it for non-user-navigation layers such as loading/fade/system effects.")]
        [SerializeField]
        private bool _recordOpenedPanelsInNavigationHistory = true;

        private IPresentationRoot _root;
        private IPanelLayer _navigationParent;
        private bool _navigationParentResolved;

        public Transform ContentRoot => _contentRoot
            ? _contentRoot
            : throw new InvalidOperationException($"{GetType().Name} requires assigned content root.");

        public Transform CacheRoot => _cacheRoot
            ? _cacheRoot
            : throw new InvalidOperationException($"{GetType().Name} requires assigned cache root.");

        public bool RecordOpenedPanelsInNavigationHistory => _recordOpenedPanelsInNavigationHistory;

        public IPanelLayer NavigationParent
        {
            get
            {
                if (_navigationParentResolved)
                    return _navigationParent;

                _navigationParent = FindNavigationParent();
                _navigationParentResolved = true;

                return _navigationParent;
            }
        }

        private IPresentationRoot Root
        {
            get
            {
                if (_root != null)
                    return _root;

                _root = GetComponentInParent<PresentationRoot>();

                return _root ?? throw new InvalidOperationException(
                    $"{GetType().Name} must be placed under {nameof(PresentationRoot)}.");
            }
        }

        public bool IsNavigationDescendantOf(IPanelLayer layer)
        {
            if (layer == null)
                return false;

            var current = NavigationParent;

            while (current != null)
            {
                if (ReferenceEquals(current, layer))
                    return true;

                current = current.NavigationParent;
            }

            return false;
        }

        protected virtual void Awake()
        {
            _ = Root;
            _ = NavigationParent;

            ValidateRoots();
        }

        public abstract bool CanHandle(Panel panel);

        public abstract void Open(IPanelRuntimeHandle handle);

        public abstract void Close(IPanelRuntimeHandle handle);

        public abstract void Release(IPanelRuntimeHandle handle);

        private IPanelLayer FindNavigationParent()
        {
            var current = transform.parent;

            while (current)
            {
                var behaviours = current.GetComponents<MonoBehaviour>();

                foreach (var behaviour in behaviours)
                {
                    if (behaviour is IPanelLayer layer)
                        return layer;
                }

                current = current.parent;
            }

            return null;
        }

        private void ValidateRoots()
        {
            if (_contentRoot == false)
                throw new InvalidOperationException(
                    $"{GetType().Name} on '{name}' requires assigned content root.");

            if (_cacheRoot == false)
                throw new InvalidOperationException(
                    $"{GetType().Name} on '{name}' requires assigned cache root.");

            if (ReferenceEquals(_contentRoot, _cacheRoot))
                throw new InvalidOperationException(
                    $"{GetType().Name} on '{name}' must use different content and cache roots.");
        }
    }

    public abstract class PanelLayer<TPanel> : PanelLayer where TPanel : class, IPanel
    {
        public override bool CanHandle(Panel panel) => panel && panel is TPanel;
    }
}