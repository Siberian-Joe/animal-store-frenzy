using Cysharp.Threading.Tasks;
using NewCore.Services;
using Zenject;

namespace NewCore.Bootstrap
{
    public sealed class ApplicationBootstrapper : IInitializable
    {
        private readonly ISceneLoader _sceneLoader;

        public ApplicationBootstrapper(ISceneLoader sceneLoader) => _sceneLoader = sceneLoader;

        public void Initialize() => _sceneLoader.LoadSceneAsync(SceneIdentifier.MainMenu).Forget();
    }
}