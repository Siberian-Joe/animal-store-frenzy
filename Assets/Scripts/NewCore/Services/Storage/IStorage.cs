using System.Threading;
using Cysharp.Threading.Tasks;
using NewCore.Domain;
using R3;

namespace NewCore.Services.Storage
{
    public interface IStorage
    {
        UniTask<Result<bool>> ExistsAsync(string key, CancellationToken cancellationToken = default);

        UniTask<Result<T>> LoadAsync<T>(string key, CancellationToken cancellationToken = default)
            where T : IModel;

        UniTask<Result<Unit>> SaveAsync<T>(string key, T data, CancellationToken cancellationToken = default);
    }
}