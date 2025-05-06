using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using NewCore.Data;
using NewCore.Domain;
using R3;

namespace NewCore.Services
{
    public interface IGameDataService : IGameDataResolver
    {
        void Register<TModel, TProxy>(string key, Func<TModel> createDefault, Func<TModel, TProxy> createProxy)
            where TModel : IModel
            where TProxy : IProxy;

        UniTask<Result<TProxy>> LoadAsync<TModel, TProxy>(CancellationToken cancellationToken = default)
            where TModel : IModel
            where TProxy : IProxy;

        UniTask<Result<Unit>> SaveAsync<TModel, TProxy>(CancellationToken cancellationToken = default)
            where TModel : IModel
            where TProxy : IProxy;

        UniTask<Result<Unit>> ResetAsync<TModel, TProxy>(CancellationToken cancellationToken = default)
            where TModel : IModel
            where TProxy : IProxy;
    }
}