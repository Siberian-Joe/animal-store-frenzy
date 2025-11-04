using System.Collections.Generic;
using System.Linq;
using NewCore.Data;
using NewCore.Domain;
using NewCore.Modules.Interaction;

namespace NewCore.Extensions
{
    public static class ProxyExtensions
    {
        public static TProxy ToProxy<TProxy>(
            this IModel model,
            IProxyFactory factory)
            where TProxy : IProxy =>
            factory.Create<TProxy>(model);

        public static IEnumerable<TProxy> ToProxies<TProxy>(
            this IEnumerable<IModel> models,
            IProxyFactory factory)
            where TProxy : IProxy =>
            models.Select(factory.Create<TProxy>);

        public static IEnumerable<TModel> ToModels<TModel>(this IEnumerable<Proxy<TModel>> proxies)
            where TModel : IModel =>
            proxies
                .Select(proxy => proxy.ToModel());


    }
}