using System;
using System.Collections.Generic;
using Modules.SceneNavigation.Runtime.Configurations;
using Modules.SceneNavigation.Runtime.Contracts;
using UnityEngine.AddressableAssets;

namespace Modules.SceneNavigation.Runtime.Catalogs
{
    public sealed class SceneCatalog : ISceneCatalog
    {
        private Dictionary<Type, AssetReference> _referencesByRouteType;
        private AssetReference _tempSceneReference;

        public AssetReference TempSceneReference
        {
            get
            {
                EnsureInitialized();
                return _tempSceneReference;
            }
        }

        public void Initialize(ISceneNavigationConfig config)
        {
            if (_referencesByRouteType != null)
                throw new InvalidOperationException(
                    $"{nameof(SceneCatalog)} is already initialized.");

            if (config == null)
                throw new ArgumentNullException(nameof(config));

            config.Validate();

            var references = new Dictionary<Type, AssetReference>();

            foreach (var definition in config.Scenes)
            {
                if (definition == false)
                    throw new InvalidOperationException(
                        $"{nameof(SceneCatalog)} contains a null scene definition.");

                definition.Validate();

                if (references.TryAdd(definition.RouteType, definition.SceneReference) == false)
                    throw new InvalidOperationException(
                        $"Duplicate scene route '{definition.RouteType.FullName}' in scene catalog.");
            }

            _tempSceneReference = config.TempSceneReference;
            _referencesByRouteType = references;
        }

        public AssetReference GetReference<TRoute>() where TRoute : SceneRoute =>
            GetReference(typeof(TRoute));

        public AssetReference GetReference(Type routeType)
        {
            if (routeType == null)
                throw new ArgumentNullException(nameof(routeType));

            if (typeof(SceneRoute).IsAssignableFrom(routeType) == false)
                throw new InvalidOperationException(
                    $"Type '{routeType.FullName}' is not a {nameof(SceneRoute)}.");

            EnsureInitialized();

            return _referencesByRouteType.TryGetValue(routeType, out var reference)
                ? reference
                : throw new InvalidOperationException(
                    $"Scene catalog does not contain route '{routeType.FullName}'.");
        }

        private void EnsureInitialized()
        {
            if (_referencesByRouteType == null)
                throw new InvalidOperationException(
                    $"{nameof(SceneCatalog)} was used before initialization.");
        }
    }
}