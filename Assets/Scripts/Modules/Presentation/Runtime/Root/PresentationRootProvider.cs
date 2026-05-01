using System;
using Modules.ResourceLoading.Runtime.Contracts;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Modules.Presentation.Runtime.Root
{
    public sealed class PresentationRootProvider : IPresentationRootAccess, IPresentationRootInitializer, IDisposable
    {
        private PresentationRoot _root;
        private IResourceLease<GameObject> _prefabLease;

        public bool IsInitialized => _root;

        public IPresentationRoot Root => _root == false
            ? throw new InvalidOperationException($"{nameof(PresentationRootProvider)} was used before initialization.")
            : _root;

        public void Initialize(
            PresentationRoot root,
            IResourceLease<GameObject> prefabLease = null)
        {
            if (_root)
                throw new InvalidOperationException($"{nameof(PresentationRootProvider)} is already initialized.");

            _root = root ? root : throw new ArgumentNullException(nameof(root));
            _prefabLease = prefabLease;
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