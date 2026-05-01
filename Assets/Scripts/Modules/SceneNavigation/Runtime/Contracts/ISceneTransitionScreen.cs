using System.Threading;
using Cysharp.Threading.Tasks;

namespace Modules.SceneNavigation.Runtime.Contracts
{
    public interface ISceneTransitionScreen
    {
        UniTask ShowAsync(CancellationToken token);

        UniTask HideAsync(CancellationToken token);
    }
}