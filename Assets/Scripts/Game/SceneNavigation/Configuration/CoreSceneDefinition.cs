using Game.SceneNavigation.Runtime.Routes;
using Modules.SceneNavigation.Runtime.Configurations;
using UnityEngine;

namespace Game.SceneNavigation.Runtime.Configuration
{
    [CreateAssetMenu(
        fileName = "CoreSceneDefinition",
        menuName = "Game/Scene Navigation/Core Scene Definition")]
    public sealed class CoreSceneDefinition : SceneDefinition<CoreRoute>
    {
    }
}