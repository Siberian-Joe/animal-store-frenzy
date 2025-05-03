using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using NewCore.Services.Scenes;
using Zenject;

namespace NewCore.Bootstrap
{
    public sealed class ApplicationBootstrapper : IInitializable, IDisposable
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly CancellationTokenSource _cancellationTokenSource = new();

        public ApplicationBootstrapper(ISceneLoader sceneLoader) => _sceneLoader = sceneLoader;

        public void Initialize() =>
            _sceneLoader.LoadSceneAsync(SceneIdentifier.MainMenu, _cancellationTokenSource.Token).Forget();

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
    }
}