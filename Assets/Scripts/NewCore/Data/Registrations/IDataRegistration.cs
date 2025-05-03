using System;
using NewCore.Data;
using NewCore.Domain;

namespace NewCore.Services
{
    public interface IDataRegistration
    {
        string Key { get; }
    }

    public interface IDataRegistration<TModel, out TProxy> : IDataRegistration
        where TModel : IModel
        where TProxy : IProxy
    {
        Func<TModel> CreateDefault { get; }
        Func<TModel, TProxy> CreateProxy { get; }
    }
}