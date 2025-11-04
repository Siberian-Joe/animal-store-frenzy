using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using NewCore.Data;
using NewCore.Domain;
using NewCore.Extensions;
using NewCore.Modules.Interaction;
using NewCore.Services.Storage;
using R3;
using UnityEngine;
using Zenject;

namespace NewCore.Services.GameData
{
    public sealed class GameDataService : IGameDataService, IInitializable
    {
        private readonly IStorage _storage;
        private readonly IProxyFactory _proxyFactory;

        private readonly Dictionary<Type, IDataRegistration> _registrations = new();
        private readonly Dictionary<Type, IProxy> _proxyCache = new();

        public GameDataService(IStorage storage, IProxyFactory proxyFactory)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
            _proxyFactory = proxyFactory ?? throw new ArgumentNullException(nameof(proxyFactory));
        }

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

        public void Register<TModel, TProxy>(
            string key,
            Func<TModel> createDefault,
            Func<TModel, IProxyFactory, TProxy> createProxy)
            where TModel : IModel
            where TProxy : IProxy =>
            _registrations[typeof(TModel)] =
                new DataRegistration<TModel, TProxy>(key, createDefault, createProxy);

        public UniTask<Result<TProxy>> LoadAsync<TModel, TProxy>(CancellationToken cancellationToken = default)
            where TModel : IModel
            where TProxy : IProxy =>
            LoadAsync<TModel, TProxy>(_proxyFactory, cancellationToken);

        public async UniTask<Result<TProxy>> LoadAsync<TModel, TProxy>(
            IProxyFactory factory,
            CancellationToken cancellationToken = default)
            where TModel : IModel
            where TProxy : IProxy
        {
            if (factory == null)
                return Result<TProxy>.Fail(new DataError($"{nameof(IProxyFactory)} is null"));

            var modelType = typeof(TModel);

            if (_proxyCache.TryGetValue(modelType, out var cached))
                return Result<TProxy>.Ok((TProxy)cached);

            if (!_registrations.TryGetValue(modelType, out var baseReg) ||
                baseReg is not IDataRegistration<TModel, TProxy> registration)
                return Result<TProxy>.Fail(new DataError($"No registration for {modelType.Name}"));

            if (cancellationToken.IsCancellationRequested)
                return await UniTask.FromCanceled<Result<TProxy>>(cancellationToken);

            var exists = await _storage.ExistsAsync(registration.Key, cancellationToken);
            if (exists.IsSuccess == false)
                return Result<TProxy>.Fail(exists.Error);

            TModel model;
            if (exists.Value)
            {
                var load = await _storage.LoadAsync<TModel>(registration.Key, cancellationToken);
                if (load.IsSuccess == false)
                    return Result<TProxy>.Fail(load.Error);

                model = load.Value;
            }
            else
            {
                model = registration.CreateDefault();
                var saveDefault = await _storage.SaveAsync(registration.Key, model, cancellationToken);
                if (saveDefault.IsSuccess == false)
                    return Result<TProxy>.Fail(saveDefault.Error);
            }

            var proxy = registration.CreateProxy(model, factory);
            _proxyCache[modelType] = proxy;
            return Result<TProxy>.Ok(proxy);
        }

        public async UniTask<Result<Unit>> SaveAsync<TModel, TProxy>(
            CancellationToken cancellationToken = default)
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

        public async UniTask<Result<Unit>> ResetAsync<TModel, TProxy>(
            CancellationToken cancellationToken = default)
            where TModel : IModel
            where TProxy : IProxy
        {
            var modelType = typeof(TModel);

            if (!_registrations.TryGetValue(modelType, out var baseReg) ||
                baseReg is not IDataRegistration<TModel, TProxy> reg)
                return Result.Fail(new DataError($"No registration for {modelType.Name}"));

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
                nameof(GameStateData),
                () => new GameStateData
                {
                    Customers = new List<CustomerData>
                    {
                        new()
                        {
                            Id = Guid.NewGuid()
                                     .ToString(),
                            Type = "FirstCustomer",
                            Position = new Vector2(0, 0)
                        },
                        new()
                        {
                            Id = Guid.NewGuid()
                                     .ToString(),
                            Type = "SecondCustomer",
                            Position = new Vector2(1, 0)
                        }
                    }
                },
                (state, factory) => state.ToProxy<GameState>(factory));

            Register(
                nameof(GameSettingsState),
                () => new GameSettingsState
                {
                    MusicVolume = 1,
                    SfxVolume = 1
                },
                (state, factory) => state.ToProxy<GameSettings>(factory));
        }
    }
}