using Cysharp.Threading.Tasks;

namespace NewCore.Services
{
    public interface ISceneLoader
    {
        UniTask LoadSceneAsync(string sceneName);
        UniTask UnloadSceneAsync(string sceneName);
    }
}