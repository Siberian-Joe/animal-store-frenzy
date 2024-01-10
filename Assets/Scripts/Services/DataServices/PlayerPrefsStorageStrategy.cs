using Interfaces.Services.DataServices;
using UnityEngine;

namespace Services.DataServices
{
    public class PlayerPrefsStorageStrategy : IStorageStrategy
    {
        public void Save(string key, string data)
        {
            PlayerPrefs.SetString(key, data);
            PlayerPrefs.Save();
        }

        public string Load(string key, string defaultValue)
        {
            return PlayerPrefs.HasKey(key) ? PlayerPrefs.GetString(key) : defaultValue;
        }
    }
}