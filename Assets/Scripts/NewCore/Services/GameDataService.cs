using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NewCore.Data;
using UnityEngine;
using Zenject;

namespace NewCore.Services
{
    public sealed class GameDataService : IGameDataService, IInitializable
    {
        private readonly IDataStorage _storage;
        private readonly Dictionary<Type, IDataHandler> _handlers = new();

        public GameDataService(IDataStorage storage) => _storage = storage;

        public void RegisterDataHandler<TState, TProxy>(string key, Func<TState> createDefaultState,
            Func<TState, TProxy> createProxyFromState)
            where TState : class, new()
            where TProxy : IProxy
        {
            var handler = new GenericDataHandler<TState, TProxy>(key, createDefaultState, createProxyFromState);
            _handlers[typeof(TState)] = handler;
        }

        public bool TryRetrieveCachedData<TState, TProxy>(out TProxy proxy)
            where TState : class, new()
            where TProxy : IProxy
        {
            proxy = default;

            if (!_handlers.TryGetValue(typeof(TState), out var handler))
            {
                Debug.LogError($"Handler not found for type: {typeof(TState).Name}");
                return false;
            }

            if (handler is not GenericDataHandler<TState, TProxy> typedHandler)
            {
                Debug.LogError($"Handler for type: {typeof(TState).Name} is not of the expected type.");
                return false;
            }

            if (typedHandler.Proxy == null)
            {
                Debug.LogError($"Data for type: {typeof(TState).Name} is not loaded.");
                return false;
            }

            proxy = typedHandler.Proxy;
            return true;
        }

        public async UniTask<TProxy> LoadAsync<TState, TProxy>() where TState : class, new() where TProxy : IProxy
        {
            if (!_handlers.TryGetValue(typeof(TState), out var handler))
            {
                Debug.LogError($"No handler registered for data {typeof(TState).Name}");
                return default;
            }

            var typedHandler = (GenericDataHandler<TState, TProxy>)handler;
            return await typedHandler.LoadAsync(_storage);
        }

        public async UniTask<bool> TrySaveAsync<TState, TProxy>() where TState : class, new() where TProxy : IProxy
        {
            if (!_handlers.TryGetValue(typeof(TState), out var handler))
            {
                Debug.LogError($"No handler registered for data {typeof(TState).Name}");
                return false;
            }

            var typedHandler = (GenericDataHandler<TState, TProxy>)handler;
            return await typedHandler.TrySaveAsync(_storage);
        }

        public async UniTask<bool> TryResetAsync<TState, TProxy>()
            where TState : class, new() where TProxy : IProxy
        {
            if (!_handlers.TryGetValue(typeof(TState), out var handler))
            {
                Debug.LogError($"No handler registered for data {typeof(TState).Name}");
                return false;
            }

            var typedHandler = (GenericDataHandler<TState, TProxy>)handler;
            return await typedHandler.TryResetAsync(_storage);
        }

        // TODO: Need to extract this to a separate module that will load initial data. For now, it's just a placeholder
        public void Initialize()
        {
            RegisterDataHandler(
                nameof(GameState),
                () => new GameState
                {
                    Customers = new List<Domain.Customer>
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
                state => new GameStateProxy(state)
            );

            RegisterDataHandler(
                nameof(GameSettingsState),
                () => new GameSettingsState
                {
                    MusicVolume = 1,
                    SfxVolume = 1
                },
                state => new GameSettingsProxy(state)
            );
        }
    }
}