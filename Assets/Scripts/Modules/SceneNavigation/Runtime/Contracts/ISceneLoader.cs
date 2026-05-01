using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Modules.SceneNavigation.Runtime.Contracts
{
    public interface ISceneLoader
    {
        UniTask<Scene> LoadSingleAsync(AssetReference reference, CancellationToken token);
    }
}