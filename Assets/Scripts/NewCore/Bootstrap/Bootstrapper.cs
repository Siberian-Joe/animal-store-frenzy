using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace NewCore.Bootstrap
{
    public abstract class Bootstrapper : IAsyncSceneBootstrapper
    {
        protected CompositeDisposable Disposables { get; } = new();

        public async UniTask InitializeAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await InitializeInternalAsync(cancellationToken);
            }
            catch (Exception exception)
            {
                Debug.LogError($"{GetType().Name} failed: {exception}");
                throw;
            }
        }

        protected abstract UniTask InitializeInternalAsync(CancellationToken cancellationToken = default);

        public virtual void Dispose() => Disposables.Dispose();
    }
}