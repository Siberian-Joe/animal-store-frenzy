using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using NewCore.Data;
using NewCore.Domain;
using NewCore.Modules.Interaction;
using R3;

namespace NewCore.Services.GameData
{
    public sealed class RuntimeScopedGameDataService : IGameDataService
    {
        private readonly IGameDataService _inner;
        private readonly IProxyFactory _runtimeFactory;

        public RuntimeScopedGameDataService(IGameDataService inner, IProxyFactory runtimeFactory)
        {
            _inner = inner;
            _runtimeFactory = runtimeFactory;
        }

        public void Register<TModel, TProxy>(
            string key,
            Func<TModel> createDefault,
            Func<TModel, IProxyFactory, TProxy> createProxy)
            where TModel : IModel
            where TProxy : IProxy =>
            _inner.Register(key, createDefault, createProxy);

        public UniTask<Result<TProxy>> LoadAsync<TModel, TProxy>(CancellationToken cancellationToken = default)
            where TModel : IModel
            where TProxy : IProxy =>
            _inner.LoadAsync<TModel, TProxy>(_runtimeFactory, cancellationToken);

        public UniTask<Result<TProxy>> LoadAsync<TModel, TProxy>(
            IProxyFactory factory,
            CancellationToken cancellationToken = default)
            where TModel : IModel
            where TProxy : IProxy =>
            _inner.LoadAsync<TModel, TProxy>(factory, cancellationToken);

        public UniTask<Result<Unit>> SaveAsync<TModel, TProxy>(CancellationToken cancellationToken = default)
            where TModel : IModel
            where TProxy : IProxy =>
            _inner.SaveAsync<TModel, TProxy>(cancellationToken);

        public UniTask<Result<Unit>> ResetAsync<TModel, TProxy>(CancellationToken cancellationToken = default)
            where TModel : IModel
            where TProxy : IProxy =>
            _inner.ResetAsync<TModel, TProxy>(cancellationToken);

        public bool TryResolve<TModel, TProxy>(out TProxy proxy)
            where TModel : IModel
            where TProxy : IProxy =>
            _inner.TryResolve<TModel, TProxy>(out proxy);
    }
}