using System;
using NewCore.Data;
using NewCore.Domain;
using NewCore.Modules.Interaction;

namespace NewCore.Services
{
    public interface IDataRegistration
    {
        string Key { get; }
        Type ModelType { get; }
        Type ProxyType { get; }
    }

    public interface IDataRegistration<TModel, TProxy> : IDataRegistration
        where TModel : IModel
        where TProxy : IProxy
    {
        Func<TModel> CreateDefault { get; }
        Func<TModel, IProxyFactory, TProxy> CreateProxy { get; }
    }
}