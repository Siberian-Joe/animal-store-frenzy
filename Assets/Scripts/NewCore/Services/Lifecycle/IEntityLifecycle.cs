using System;
using NewCore.Data;
using NewCore.ViewModels.World;
using ObservableCollections;

namespace NewCore.Services.Lifecycle
{
    public interface IEntityLifecycle<TProxy, TViewModel> : IDisposable
        where TProxy : Proxy, IEntityProxy
        where TViewModel : EntityViewModel<TProxy>
    {
        IObservableCollection<TViewModel> Entities { get; }

        void Initialize(IReadOnlyObservableList<TProxy> proxies);
    }
}