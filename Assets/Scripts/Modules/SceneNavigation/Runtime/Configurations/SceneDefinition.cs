using System;
using Modules.SceneNavigation.Runtime.Contracts;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Modules.SceneNavigation.Runtime.Configurations
{
    public abstract class SceneDefinition : ScriptableObject
    {
        [SerializeField] private AssetReference _sceneReference;

        public abstract Type RouteType { get; }

        public AssetReference SceneReference => _sceneReference;

        public void Validate()
        {
            if (_sceneReference == null || _sceneReference.RuntimeKeyIsValid() == false)
                throw new InvalidOperationException($"{name} requires a valid addressable scene reference.");
        }
    }

    public abstract class SceneDefinition<TRoute> : SceneDefinition where TRoute : SceneRoute
    {
        public sealed override Type RouteType => typeof(TRoute);
    }
}