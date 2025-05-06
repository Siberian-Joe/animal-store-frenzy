using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using NewCore.Data;
using NewCore.Domain;
using NewCore.Services.Storage;
using R3;
using UnityEngine;
using Zenject;

namespace NewCore.Services
{
    public sealed class GameDataService : IGameDataService, IInitializable
    {
        private readonly IStorage _storage;
        private readonly Dictionary<Type, IDataRegistration> _registrations = new();
        private readonly Dictionary<Type, IProxy> _proxyCache = new();

        public GameDataService(IStorage storage) =>
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));

        public bool TryResolve<TModel, TProxy>(out TProxy proxy)
            where TModel : IModel
            where TProxy : IProxy
        {
            if (_proxyCache.TryGetValue(typeof(TModel), out var value) && value is TProxy cast)
            {
                proxy = cast;
                return true;
            }

            proxy = default;
            return false;
        }

        public void Register<TModel, TProxy>(string key, Func<TModel> createDefault, Func<TModel, TProxy> createProxy)
            where TModel : IModel
            where TProxy : IProxy =>
            _registrations[typeof(TModel)] = new DataRegistration<TModel, TProxy>(key, createDefault, createProxy);

        public async UniTask<Result<TProxy>> LoadAsync<TModel, TProxy>(CancellationToken cancellationToken = default)
            where TModel : IModel
            where TProxy : IProxy
        {
            var modelType = typeof(TModel);

            if (_proxyCache.TryGetValue(modelType, out var cached))
                return Result<TProxy>.Ok((TProxy)cached);

            if (!_registrations.TryGetValue(modelType, out var baseReg) ||
                baseReg is not IDataRegistration<TModel, TProxy> registration)
                return Result<TProxy>.Fail(new DataError($"No registration for {modelType.Name}"));

            if (cancellationToken.IsCancellationRequested)
                return await UniTask.FromCanceled<Result<TProxy>>(cancellationToken);

            var exists = await _storage.ExistsAsync(registration.Key, cancellationToken);
            if (!exists.IsSuccess)
                return Result<TProxy>.Fail(exists.Error);

            TModel model;
            if (exists.Value)
            {
                var result = await _storage.LoadAsync<TModel>(registration.Key, cancellationToken);
                if (!result.IsSuccess)
                    return Result<TProxy>.Fail(result.Error);

                model = result.Value;
            }
            else
            {
                model = registration.CreateDefault();
                var result = await _storage.SaveAsync(registration.Key, model, cancellationToken);
                if (!result.IsSuccess)
                    return Result<TProxy>.Fail(result.Error);
            }

            var proxy = registration.CreateProxy(model);
            _proxyCache[modelType] = proxy;
            return Result<TProxy>.Ok(proxy);
        }

        public async UniTask<Result<Unit>> SaveAsync<TModel, TProxy>(CancellationToken cancellationToken = default)
            where TModel : IModel
            where TProxy : IProxy
        {
            var modelType = typeof(TModel);

            if (!_registrations.TryGetValue(modelType, out var baseReg))
                return Result.Fail(new DataError($"No registration for {modelType.Name}"));

            if (!_proxyCache.TryGetValue(modelType, out var proxyObj) || proxyObj is not Proxy<TModel> proxy)
                return Result.Fail(new DataError($"Proxy for {modelType.Name} not loaded"));

            if (cancellationToken.IsCancellationRequested)
                return await UniTask.FromCanceled<Result<Unit>>(cancellationToken);

            return await _storage.SaveAsync(baseReg.Key, proxy.ToModel(), cancellationToken);
        }

        public async UniTask<Result<Unit>> ResetAsync<TModel, TProxy>(CancellationToken cancellationToken = default)
            where TModel : IModel
            where TProxy : IProxy
        {
            var modelType = typeof(TModel);

            if (!_registrations.TryGetValue(modelType, out var baseReg) ||
                baseReg is not IDataRegistration<TModel, TProxy> reg)
            {
                return Result.Fail(new DataError($"No registration for {modelType.Name}"));
            }

            if (cancellationToken.IsCancellationRequested)
                return await UniTask.FromCanceled<Result<Unit>>(cancellationToken);

            var defaultModel = reg.CreateDefault();
            _proxyCache.Remove(modelType);

            return await _storage.SaveAsync(reg.Key, defaultModel, cancellationToken);
        }

        // TODO: Need to extract this to a separate module that will load initial data. For now, it's just a placeholder
        public void Initialize()
        {
            Register(
                nameof(GameState),
                () => new GameState
                {
                    Customers = new List<Customer>
                    {
                        new()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Type = "FirstCustomer",
                            Position = new Vector3Int(0, 0, 0)
                        },
                        new()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Type = "SecondCustomer",
                            Position = new Vector3Int(1, 0, 0)
                        }
                    }
                },
                state =>
                {
                    var proxy = new GameStateProxy();
                    proxy.Initialize(state);
                    return proxy;
                });

            Register(
                nameof(GameSettingsState),
                () => new GameSettingsState
                {
                    MusicVolume = 1,
                    SfxVolume = 1
                },
                state =>
                {
                    var proxy = new GameSettingsProxy();
                    proxy.Initialize(state);
                    return proxy;
                }
            );
        }
    }
}