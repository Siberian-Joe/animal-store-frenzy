using System.Collections.Generic;
using System.Linq;
using NewCore.Data;
using NewCore.Domain;

namespace NewCore.Extensions
{
    public static class ProxyExtensions
    {
        public static TProxy ToProxy<TModel, TProxy>(this TModel model)
            where TModel : IModel
            where TProxy : Proxy<TModel>, new()
        {
            var proxy = new TProxy();
            proxy.Initialize(model);
            return proxy;
        }

        public static List<TProxy> ToProxies<TModel, TProxy>(this IEnumerable<TModel> models)
            where TModel : EntityData
            where TProxy : Entity<TModel>, new() =>
            models.Select(ToProxy<TModel, TProxy>).ToList();

        public static List<TModel> ToModels<TModel, TProxy>(this IEnumerable<TProxy> proxies)
            where TModel : EntityData
            where TProxy : Entity<TModel> =>
            proxies.Select(proxy => proxy.ToModel()).ToList();
    }
}