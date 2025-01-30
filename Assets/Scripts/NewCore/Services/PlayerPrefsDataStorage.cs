using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace NewCore.Services
{
    public sealed class PlayerPrefsDataStorage : IDataStorage
    {
        public async UniTask<bool> TrySaveAsync<T>(string key, T data)
        {
            try
            {
                var json = JsonUtility.ToJson(data, true);
                PlayerPrefs.SetString(key, json);
                PlayerPrefs.Save();
                return await UniTask.FromResult(true);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error saving data for key {key}: {ex.Message}");
                return await UniTask.FromResult(false);
            }
        }

        public async UniTask<T> LoadAsync<T>(string key) where T : class, new()
        {
            var json = PlayerPrefs.GetString(key);
            var data = JsonUtility.FromJson<T>(json);
            
            Debug.Log($"Loaded data for key {key}: {json}");
            return await UniTask.FromResult(data);
        }
        
        public async UniTask<bool> ExistsAsync(string key) => await UniTask.FromResult(PlayerPrefs.HasKey(key));
    }
}