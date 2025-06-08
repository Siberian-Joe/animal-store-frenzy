using System;
using NewCore.Domain;

namespace NewCore.Data
{
    public interface IProxy : IDisposable
    {
    }

    public interface IProxy<TModel> : IProxy where TModel : IModel
    {
        TModel ToModel();
        void Initialize(TModel model);
    }
}