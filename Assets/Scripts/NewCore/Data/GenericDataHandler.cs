using System;
using Cysharp.Threading.Tasks;
using NewCore.Services;
using UnityEngine;

namespace NewCore.Data
{
    public class GenericDataHandler<TState, TProxy> : IDataHandler<TProxy>
        where TState : class, new()
        where TProxy : IProxy
    {
        public TProxy Proxy { get; private set; }

        private readonly string _key;
        private readonly Func<TState> _createDefaultState;
        private readonly Func<TState, TProxy> _createProxyFromState;

        private TState _currentState;

        public GenericDataHandler(string key, Func<TState> createDefaultState,
            Func<TState, TProxy> createProxyFromState)
        {
            if (string.IsNullOrEmpty(key))
            {
                Debug.LogError("Key cannot be null or empty.");
                return;
            }

            if (createDefaultState == null)
            {
                Debug.LogError("createDefaultState cannot be null.");
                return;
            }

            if (createProxyFromState == null)
            {
                Debug.LogError("createProxyFromState cannot be null.");
                return;
            }

            _key = key;
            _createDefaultState = createDefaultState;
            _createProxyFromState = createProxyFromState;
        }

        public async UniTask<TProxy> LoadAsync(IDataStorage storage)
        {
            if (storage == null)
            {
                Debug.LogError("Storage cannot be null.");
                return default;
            }

            if (await storage.ExistsAsync(_key))
            {
                await LoadStateFromStorageAsync(storage);
            }
            else
            {
                await InitializeDefaultStateAsync(storage);
            }

            return Proxy;
        }

        public async UniTask<bool> TrySaveAsync(IDataStorage storage)
        {
            if (storage == null)
            {
                Debug.LogError("Storage cannot be null.");
                return false;
            }

            if (_currentState == null)
            {
                Debug.LogError("State is not initialized. Cannot save.");
                return false;
            }

            return await storage.TrySaveAsync(_key, _currentState);
        }

        public async UniTask<bool> TryResetAsync(IDataStorage storage)
        {
            if (storage == null)
            {
                Debug.LogError("Storage cannot be null.");
                return false;
            }

            _currentState = _createDefaultState();
            if (_currentState == null)
            {
                Debug.LogError("Failed to create default state.");
                return false;
            }

            Proxy = _createProxyFromState(_currentState);
            return await TrySaveAsync(storage);
        }

        private async UniTask LoadStateFromStorageAsync(IDataStorage storage)
        {
            try
            {
                _currentState = await storage.LoadAsync<TState>(_key);
                Proxy = _createProxyFromState(_currentState);
            }
            catch (Exception exception)
            {
                Debug.LogError($"Failed to load state from storage: {exception.Message}");
                _currentState = null;
                Proxy = default;
            }
        }

        private async UniTask InitializeDefaultStateAsync(IDataStorage storage)
        {
            try
            {
                _currentState = _createDefaultState();
                Proxy = _createProxyFromState(_currentState);
                await TrySaveAsync(storage);
            }
            catch (Exception exception)
            {
                Debug.LogError($"Failed to initialize default state: {exception.Message}");
                _currentState = null;
                Proxy = default;
            }
        }
    }
}