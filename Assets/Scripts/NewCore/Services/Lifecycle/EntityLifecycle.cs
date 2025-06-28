using System;
using System.Collections.Generic;
using NewCore.Data;
using NewCore.Extensions;
using NewCore.Factories;
using NewCore.ViewModels;
using ObservableCollections;
using R3;

namespace NewCore.Services.Lifecycle
{
    public abstract class EntityLifecycle<TProxy, TViewModel> : IDisposable
        where TProxy : IEntityProxy, new()
        where TViewModel : EntityViewModel<TProxy>
    {
        public IObservableCollection<TViewModel> Entities => _viewModels;

        protected readonly CompositeDisposable Disposables = new();

        private readonly ObservableList<TViewModel> _viewModels = new();
        private readonly Dictionary<string, TViewModel> _map = new();
        private readonly IViewModelFactory _viewModelFactory;

        protected EntityLifecycle(IViewModelFactory viewModelFactory) => _viewModelFactory = viewModelFactory;

        public virtual void Initialize(IReadOnlyObservableList<TProxy> proxies)
        {
            foreach (var proxy in proxies)
                Add(proxy);

            proxies
                .ObserveAdd()
                .Subscribe(addEvent => Add(addEvent.Value))
                .AddTo(Disposables);

            proxies
                .ObserveRemove()
                .Subscribe(removeEvent => Remove(removeEvent.Value))
                .AddTo(Disposables);
        }

        private void Add(TProxy proxy)
        {
            if (_map.ContainsKey(proxy.ID))
                return;

            var viewModel = _viewModelFactory.Create<TProxy, TViewModel>(proxy);
            _map.Add(proxy.ID, viewModel);
            _viewModels.Add(viewModel);
        }

        private void Remove(TProxy proxy)
        {
            if (!_map.TryGetValue(proxy.ID, out var viewModel))
                return;

            _viewModels.Remove(viewModel);
            _map.Remove(proxy.ID);
            viewModel.Dispose();
        }

        public virtual void Dispose()
        {
            Disposables.Dispose();
            _viewModels.ClearAndDispose();
        }
    }
}