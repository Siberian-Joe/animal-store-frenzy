using Game.SceneNavigation.Routes;
using Modules.SceneNavigation.Runtime.Configurations;
using UnityEngine;

namespace Game.SceneNavigation.Configuration
{
    [CreateAssetMenu(
        fileName = "MainMenuSceneDefinition",
        menuName = "Game/Scene Navigation/Main Menu Scene Definition")]
    public sealed class MainMenuSceneDefinition : SceneDefinition<MainMenuRoute>
    {
    }
}