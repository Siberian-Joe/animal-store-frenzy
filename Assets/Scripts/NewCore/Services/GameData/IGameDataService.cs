using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using NewCore.Data;
using NewCore.Domain;
using NewCore.Modules.Interaction;
using R3;

namespace NewCore.Services.GameData
{
    public interface IGameDataService : IGameDataResolver
    {
        public void Register<TModel, TProxy>(
            string key,
            Func<TModel> createDefault,
            Func<TModel, IProxyFactory, TProxy> createProxy)
            where TModel : IModel
            where TProxy : IProxy;

        public UniTask<Result<TProxy>> LoadAsync<TModel, TProxy>(
            CancellationToken cancellationToken = default)
            where TModel : IModel
            where TProxy : IProxy;

        public UniTask<Result<TProxy>> LoadAsync<TModel, TProxy>(
            IProxyFactory factory,
            CancellationToken cancellationToken = default)
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