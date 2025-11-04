using NewCore.Data;
using NewCore.Domain;

namespace NewCore.Modules.Interaction
{
    public interface IProxyFactory
    {
        TProxy Create<TProxy>(IModel model)
            where TProxy : IProxy;
    }
}