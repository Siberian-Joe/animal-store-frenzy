using System;
using Modules.Presentation.Runtime.Root;
using Zenject;
using Object = UnityEngine.Object;

namespace Game.Presentation.Composition.Zenject
{
    public sealed class BootstrapPresentationRootPublisher : IInitializable
    {
        private readonly PresentationRoot _presentationRoot;
        private readonly IPresentationRootInitializer _presentationRootInitializer;

        public BootstrapPresentationRootPublisher(
            PresentationRoot presentationRoot,
            IPresentationRootInitializer presentationRootInitializer)
        {
            _presentationRoot = presentationRoot
                ? presentationRoot
                : throw new ArgumentNullException(nameof(presentationRoot));

            _presentationRootInitializer = presentationRootInitializer
                                           ?? throw new ArgumentNullException(nameof(presentationRootInitializer));
        }

        public void Initialize()
        {
            if (_presentationRootInitializer.IsInitialized)
                throw new InvalidOperationException(
                    $"{nameof(IPresentationRootInitializer)} is already initialized before bootstrap panel root publication. " +
                    "This usually means application presentation resources created a dynamic PanelRoot before the " +
                    "preplaced Bootstrap PanelRoot was published.");

            _presentationRoot.transform.SetParent(null, false);
            Object.DontDestroyOnLoad(_presentationRoot.gameObject);

            _presentationRootInitializer.Initialize(_presentationRoot);
        }
    }
}