using System;
using Modules.ResourceLoading.Contracts;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Modules.Presentation.Runtime.Root
{
    public sealed class PanelRootProvider : IPanelRootAccess, IPanelRootInitializer, IDisposable
    {
        private PanelRoot _root;
        private IResourceLease<GameObject> _prefabLease;

        public IPanelRoot Root => _root == false
            ? throw new InvalidOperationException($"{nameof(PanelRootProvider)} was used before initialization.")
            : _root;

        public void Initialize(PanelRoot root, IResourceLease<GameObject> prefabLease)
        {
            if (_root)
                throw new InvalidOperationException($"{nameof(PanelRootProvider)} is already initialized.");

            _root = root ? root : throw new ArgumentNullException(nameof(root));
            _prefabLease = prefabLease ?? throw new ArgumentNullException(nameof(prefabLease));
        }

        public void Dispose()
        {
            if (_root)
                Object.Destroy(_root.gameObject);

            _root = null;

            _prefabLease?.Dispose();
            _prefabLease = null;
        }
    }
}