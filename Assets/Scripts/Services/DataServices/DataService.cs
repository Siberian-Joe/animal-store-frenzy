using System.Collections.Generic;
using Interfaces.Services.DataServices;
using UnityEngine;

namespace Services.DataServices
{
    public class DataService : IDataService
    {
        private const string DataCacheKey = "DataCache";

        private readonly IStorageStrategy _storageStrategy;

        private Dictionary<string, string> _dataCache;

        public DataService(IStorageStrategy storageStrategy)
        {
            _storageStrategy = storageStrategy;
            LoadDataCache();
        }

        public void Save<T>(T data, string key = null, string id = null)
        {
            key ??= GenerateKey<T>(id);
            var jsonData = JsonUtility.ToJson(data);
            _dataCache[key] = jsonData;
            SaveDataCache();
        }

        public T Load<T>(string key = null, T defaultValue = default, string id = null)
        {
            key ??= GenerateKey<T>(id);
            return _dataCache.TryGetValue(key, out var jsonData) ? JsonUtility.FromJson<T>(jsonData) : defaultValue;
        }

        private void SaveDataCache()
        {
            var jsonData = JsonUtility.ToJson(_dataCache);
            _storageStrategy.Save(DataCacheKey, jsonData);
        }

        private void LoadDataCache()
        {
            var jsonData = _storageStrategy.Load(DataCacheKey, "{}");
            _dataCache = JsonUtility.FromJson<Dictionary<string, string>>(jsonData);
        }

        private string GenerateKey<T>(string id = null)
        {
            return $"{typeof(T).FullName}_data{id}";
        }
    }
}