using NewCore.Data;
using NewCore.Domain;

namespace NewCore.Services
{
    public interface IGameDataResolver
    {
        bool TryResolve<TModel, TProxy>(out TProxy proxy)
            where TModel : IModel
            where TProxy : IProxy;
    }
}