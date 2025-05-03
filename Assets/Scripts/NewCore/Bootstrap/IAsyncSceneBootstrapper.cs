using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace NewCore.Bootstrap
{
    public interface IAsyncSceneBootstrapper : IDisposable
    {
        UniTask InitializeAsync(CancellationToken cancellationToken = default);
    }
}