using System.Collections.Generic;
using System.Linq;
using NewCore.Data;
using NewCore.Domain;

namespace NewCore.Extensions
{
    public static class ProxyExtensions
    {
        public static TProxy ToProxy<TModel, TProxy>(this TModel model)
            where TModel : Entity
            where TProxy : EntityProxy<TModel>, new()
        {
            var proxy = new TProxy();
            proxy.Initialize(model);
            return proxy;
        }

        public static List<TProxy> ToProxies<TModel, TProxy>(this IEnumerable<TModel> models)
            where TModel : Entity
            where TProxy : EntityProxy<TModel>, new() =>
            models.Select(ToProxy<TModel, TProxy>).ToList();

        public static List<TModel> ToModels<TModel, TProxy>(this IEnumerable<TProxy> proxies)
            where TModel : Entity
            where TProxy : EntityProxy<TModel> =>
            proxies.Select(proxy => proxy.ToModel()).ToList();
    }
}