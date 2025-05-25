using NewCore.Data;
using NewCore.Domain;

namespace NewCore.Extensions
{
    public static class EntityExtensions
    {
        public static bool IsEquivalentTo<TModel>(this Entity<TModel> proxy, TModel model) where TModel : EntityData
        {
            if (proxy == null || model == null)
                return false;

            return proxy.Id == model.Id;
        }
    }
}