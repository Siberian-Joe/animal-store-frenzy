using Cysharp.Threading.Tasks;

namespace NewCore.Services
{
    public interface IDataStorage
    {
        UniTask<bool> TrySaveAsync<T>(string key, T data);
        UniTask<T> LoadAsync<T>(string key) where T : class, new();
        UniTask<bool> ExistsAsync(string key);
    }
}