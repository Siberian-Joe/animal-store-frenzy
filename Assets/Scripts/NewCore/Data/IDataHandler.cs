using Cysharp.Threading.Tasks;
using NewCore.Services;

namespace NewCore.Data
{
    public interface IDataHandler
    {
    }

    public interface IDataHandler<TProxy> : IDataHandler where TProxy : IProxy
    {
        TProxy Proxy { get; }

        UniTask<TProxy> LoadAsync(IDataStorage storage);
        UniTask<bool> TrySaveAsync(IDataStorage storage);
        UniTask<bool> TryResetAsync(IDataStorage storage);
    }
}