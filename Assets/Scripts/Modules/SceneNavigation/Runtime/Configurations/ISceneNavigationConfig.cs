using System.Collections.Generic;
using UnityEngine.AddressableAssets;

namespace Modules.SceneNavigation.Runtime.Configurations
{
    public interface ISceneNavigationConfig
    {
        AssetReference TempSceneReference { get; }

        IReadOnlyList<SceneDefinition> Scenes { get; }

        void Validate();
    }
}