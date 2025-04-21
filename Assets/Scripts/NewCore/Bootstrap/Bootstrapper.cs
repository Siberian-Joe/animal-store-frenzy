using System;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace NewCore.Bootstrap
{
    public abstract class Bootstrapper : IAsyncSceneBootstrapper
    {
        protected CompositeDisposable Disposables { get; } = new();

        public async UniTask InitializeAsync()
        {
            try
            {
                await InitializeInternalAsync();
            }
            catch (Exception exception)
            {
                Debug.LogError($"{GetType().Name} failed: {exception}");
                throw;
            }
        }

        protected abstract UniTask InitializeInternalAsync();

        public virtual void Dispose() => Disposables.Dispose();
    }
}