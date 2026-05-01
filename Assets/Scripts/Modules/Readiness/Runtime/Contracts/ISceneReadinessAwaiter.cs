using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Modules.Readiness.Runtime.Contracts
{
    public interface ISceneReadinessAwaiter
    {
        UniTask WaitReadyAsync(Scene scene, CancellationToken token);
    }
}