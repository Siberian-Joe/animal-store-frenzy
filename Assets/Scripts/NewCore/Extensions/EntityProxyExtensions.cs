using NewCore.Data;
using NewCore.Domain;

namespace NewCore.Extensions
{
    public static class EntityProxyExtensions
    {
        public static bool IsEquivalentTo<TModel>(this EntityProxy<TModel> proxy, TModel model) where TModel : Entity
        {
            if (proxy == null || model == null)
                return false;

            return proxy.Id == model.Id;
        }
    }
}