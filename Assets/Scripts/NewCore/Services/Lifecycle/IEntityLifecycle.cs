using System;
using NewCore.Data;
using NewCore.ViewModels;
using ObservableCollections;

namespace NewCore.Services.Lifecycle
{
    public interface IEntityLifecycle<TProxy, TViewModel> : IDisposable
        where TProxy : IEntityProxy, new()
        where TViewModel : EntityViewModel<TProxy>
    {
        IObservableCollection<TViewModel> Entities { get; }

        void Initialize(IReadOnlyObservableList<TProxy> proxies);
    }
}