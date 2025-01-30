using System;
using NewCore.Domain;

namespace NewCore.Data
{
    public interface IProxy : IDisposable
    {
    }

    public interface IProxy<in TModel> : IProxy where TModel : IModel
    {
        void Initialize(TModel model);
    }
}