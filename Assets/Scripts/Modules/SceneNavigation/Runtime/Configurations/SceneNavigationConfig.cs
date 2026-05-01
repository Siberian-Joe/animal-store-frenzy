using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Modules.SceneNavigation.Runtime.Configurations
{
    [CreateAssetMenu(
        fileName = "SceneNavigationConfig",
        menuName = "Game/Scene Navigation/Scene Navigation Config")]
    public sealed class SceneNavigationConfig : ScriptableObject, ISceneNavigationConfig
    {
        [SerializeField] private AssetReference _tempSceneReference;
        [SerializeField] private SceneDefinition[] _scenes;

        public AssetReference TempSceneReference => _tempSceneReference;

        public IReadOnlyList<SceneDefinition> Scenes => _scenes;

        public void Validate()
        {
            if (_tempSceneReference == null || _tempSceneReference.RuntimeKeyIsValid() == false)
            {
                throw new InvalidOperationException(
                    $"{nameof(SceneNavigationConfig)} requires valid temp scene reference.");
            }

            if (_scenes == null || _scenes.Length == 0)
            {
                throw new InvalidOperationException(
                    $"{nameof(SceneNavigationConfig)} requires at least one scene definition.");
            }

            for (var i = 0; i < _scenes.Length; i++)
            {
                var scene = _scenes[i];

                if (scene == false)
                {
                    throw new InvalidOperationException(
                        $"{nameof(SceneNavigationConfig)} contains null scene definition at index {i}.");
                }

                scene.Validate();
            }
        }
    }
}