using System;
using System.Collections.Generic;
using NewCore.Components;
using NewCore.ViewModels.World;
using NewCore.Views;
using ObservableCollections;
using R3;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NewCore.ViewBinders.Decorators
{
    public class SceneAwareViewBinderDecorator : IViewBinder, IDisposable
    {
        private readonly IViewBinder _inner;
        private readonly Dictionary<string, IView> _instances = new();
        private readonly CompositeDisposable _disposables = new();
        private bool _initialized;

        public SceneAwareViewBinderDecorator(IViewBinder inner) => _inner = inner;

        public void Bind<TViewModel, TView>(
            IObservableCollection<TViewModel> source,
            TView prefab,
            Transform parent)
            where TViewModel : IEntityViewModel
            where TView : EntityView<TViewModel>
        {
            EnsureInitialized();

            foreach (var viewModel in source)
                EnsureView(viewModel, prefab, parent);

            source.ObserveAdd()
                .Subscribe(addEvent => EnsureView(addEvent.Value, prefab, parent))
                .AddTo(_disposables);

            source.ObserveRemove()
                .Subscribe(removeEvent =>
                {
                    if (!_instances.TryGetValue(removeEvent.Value.Id, out var view) ||
                        view is not MonoBehaviour monoBehaviour)
                        return;

                    Object.Destroy(monoBehaviour.gameObject);
                    _instances.Remove(removeEvent.Value.Id);
                })
                .AddTo(_disposables);
        }

        public TView BindSingle<TViewModel, TView>(
            TViewModel viewModel,
            TView prefab,
            Transform parent)
            where TViewModel : IEntityViewModel
            where TView : EntityView<TViewModel>
        {
            EnsureInitialized();

            if (_instances.TryGetValue(viewModel.Id, out var existing))
            {
                ((TView)existing).Bind(viewModel);
                return (TView)existing;
            }

            var view = _inner.BindSingle(viewModel, prefab, parent);
            _instances[viewModel.Id] = view;
            return view;
        }

        private void EnsureInitialized()
        {
            if (_initialized) return;
            _initialized = true;

            var identifiers = Object.FindObjectsByType<EntityIdentifier>(FindObjectsSortMode.None);

            foreach (var identifier in identifiers)
            {
                var view = identifier.GetComponent<IView>();
                var id = identifier.Id;
                if (view != null && !string.IsNullOrEmpty(id))
                    _instances[id] = view;
            }
        }

        private void EnsureView<TViewModel, TView>(
            TViewModel viewModel,
            TView prefab,
            Transform parent)
            where TViewModel : IEntityViewModel
            where TView : EntityView<TViewModel>
        {
            if (_instances.TryGetValue(viewModel.Id, out var existing))
            {
                ((TView)existing).Bind(viewModel);
                return;
            }

            var view = _inner.BindSingle(viewModel, prefab, parent);
            _instances[viewModel.Id] = view;
        }

        public void Dispose()
        {
            _disposables.Dispose();
            _instances.Clear();
        }
    }
}