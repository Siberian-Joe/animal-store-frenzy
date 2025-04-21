using System;
using Cysharp.Threading.Tasks;

namespace NewCore.Bootstrap
{
    public interface IAsyncSceneBootstrapper : IDisposable
    {
        UniTask InitializeAsync();
    }
}