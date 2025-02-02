using System;
using NewCore.Data;
using NewCore.Domain;
using NewCore.ViewModels;
using ObservableCollections;

namespace NewCore.Lifecycle
{
    public interface IEntityLifecycle<TModel, TProxy, TViewModel> : IDisposable
        where TModel : Entity
        where TProxy : EntityProxy<TModel>, new()
        where TViewModel : EntityViewModel<TModel, TProxy>
    {
        IObservableCollection<TViewModel> Entities { get; }

        void Initialize(ProxyCollection<TModel, TProxy> proxies);
    }
}