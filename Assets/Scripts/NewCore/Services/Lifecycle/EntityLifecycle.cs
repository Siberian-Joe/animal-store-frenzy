using System;
using System.Collections.Generic;
using NewCore.Data;
using NewCore.Domain;
using NewCore.Factories;
using NewCore.ViewModels;
using ObservableCollections;
using R3;

namespace NewCore.Services.Lifecycle
{
    public abstract class EntityLifecycle<TModel, TProxy, TViewModel> : IDisposable
        where TModel : Entity
        where TViewModel : EntityViewModel<TModel, TProxy>
        where TProxy : EntityProxy<TModel>, new()
    {
        public IObservableCollection<TViewModel> Entities => _viewModels;

        protected readonly CompositeDisposable Disposables = new();

        private readonly ObservableList<TViewModel> _viewModels = new();
        private readonly Dictionary<string, TViewModel> _map = new();
        private readonly IViewModelFactory _viewModelFactory;

        protected EntityLifecycle(IViewModelFactory viewModelFactory) => _viewModelFactory = viewModelFactory;

        public virtual void Initialize(ProxyCollection<TModel, TProxy> proxies)
        {
            foreach (var proxy in proxies)
                Add(proxy);

            proxies
                .ObserveAdd()
                .Subscribe(proxy => Add(proxy.Value))
                .AddTo(Disposables);

            proxies
                .ObserveRemove()
                .Subscribe(proxy => Remove(proxy.Value))
                .AddTo(Disposables);
        }

        private void Add(TProxy proxy)
        {
            if (_map.ContainsKey(proxy.Id))
                return;

            var viewModel = _viewModelFactory.Create<TProxy, TViewModel>(proxy);
            _map.Add(proxy.Id, viewModel);
            _viewModels.Add(viewModel);
        }

        private void Remove(TProxy proxy)
        {
            if (!_map.TryGetValue(proxy.Id, out var viewModel))
                return;

            _viewModels.Remove(viewModel);
            _map.Remove(proxy.Id);
            viewModel.Dispose();
        }

        public virtual void Dispose() => Disposables.Dispose();
    }
}