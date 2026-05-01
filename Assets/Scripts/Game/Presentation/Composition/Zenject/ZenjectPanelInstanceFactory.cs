using System;
using Modules.Presentation.Runtime.Panels;
using Modules.Presentation.Runtime.Preparation;
using UnityEngine;
using Zenject;

namespace Game.Presentation.Composition.Zenject
{
    public sealed class ZenjectPanelInstanceFactory : IPanelInstanceFactory
    {
        private readonly IInstantiator _instantiator;

        public ZenjectPanelInstanceFactory(IInstantiator instantiator) =>
            _instantiator = instantiator ?? throw new ArgumentNullException(nameof(instantiator));

        public TPresenter CreatePresenter<TPresenter>() where TPresenter : PanelPresenter =>
            _instantiator.Instantiate<TPresenter>();

        public PanelPresenter CreatePresenter(Type presenterType)
        {
            if (presenterType == null)
                throw new ArgumentNullException(nameof(presenterType));

            if (typeof(PanelPresenter).IsAssignableFrom(presenterType) == false)
            {
                throw new InvalidOperationException(
                    $"Type '{presenterType.FullName}' is not a {nameof(PanelPresenter)}.");
            }

            return (PanelPresenter)_instantiator.Instantiate(presenterType);
        }

        public GameObject CreatePanelRoot(GameObject prefab) => prefab == false
            ? throw new ArgumentNullException(nameof(prefab))
            : _instantiator.InstantiatePrefab(prefab);

        public TComponent CreateComponent<TComponent>(GameObject prefab)
            where TComponent : Component =>
            prefab == false
                ? throw new ArgumentNullException(nameof(prefab))
                : _instantiator.InstantiatePrefabForComponent<TComponent>(prefab);
    }
}