using System;
using UnityEngine.AddressableAssets;

namespace Modules.SceneNavigation.Runtime.Contracts
{
    public interface ISceneCatalog
    {
        AssetReference TempSceneReference { get; }

        AssetReference GetReference<TRoute>()
            where TRoute : SceneRoute;

        AssetReference GetReference(Type routeType);
    }
}