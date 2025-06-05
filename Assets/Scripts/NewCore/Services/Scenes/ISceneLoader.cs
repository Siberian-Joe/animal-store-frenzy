using System.Threading;
using Cysharp.Threading.Tasks;

namespace NewCore.Services.Scenes
{
    public interface ISceneLoader
    {
        UniTask LoadSceneAsync(string sceneName, CancellationToken cancellationToken = default);
    }
}