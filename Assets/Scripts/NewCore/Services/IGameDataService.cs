using System;
using Cysharp.Threading.Tasks;
using NewCore.Data;

namespace NewCore.Services
{
    public interface IGameDataService
    {
        void RegisterDataHandler<TState, TProxy>(string key, Func<TState> createDefaultState,
            Func<TState, TProxy> createProxyFromState) where TState : class, new() where TProxy : IProxy;

        bool TryRetrieveCachedData<TState, TProxy>(out TProxy proxy) where TState : class, new() where TProxy : IProxy;

        UniTask<TProxy> LoadAsync<TState, TProxy>() where TState : class, new() where TProxy : IProxy;
        UniTask<bool> TrySaveAsync<TState, TProxy>() where TState : class, new() where TProxy : IProxy;
        UniTask<bool> TryResetAsync<TState, TProxy>() where TState : class, new() where TProxy : IProxy;
    }
}