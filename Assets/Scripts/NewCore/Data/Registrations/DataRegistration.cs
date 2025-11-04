using System;
using NewCore.Data;
using NewCore.Domain;
using NewCore.Modules.Interaction;

namespace NewCore.Services
{
    public sealed class DataRegistration<TModel, TProxy> : IDataRegistration<TModel, TProxy>
        where TModel : IModel
        where TProxy : IProxy
    {
        public string Key { get; }
        public Func<TModel> CreateDefault { get; }
        public Func<TModel, IProxyFactory, TProxy> CreateProxy { get; }

        public Type ModelType => typeof(TModel);
        public Type ProxyType => typeof(TProxy);

        public DataRegistration(
            string key,
            Func<TModel> createDefault,
            Func<TModel, IProxyFactory, TProxy> createProxy)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            CreateDefault = createDefault ?? throw new ArgumentNullException(nameof(createDefault));
            CreateProxy = createProxy ?? throw new ArgumentNullException(nameof(createProxy));
        }
    }
}