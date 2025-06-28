using System;
using System.Collections.Generic;
using NewCore.Factories;
using NewCore.ViewModels;
using NewCore.Views;
using ObservableCollections;
using R3;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NewCore.Services.ViewSynchronizer
{
    public class ViewSynchronizer : IViewSynchronizer, IDisposable
    {
        private readonly IViewFactory _viewFactory;
        private readonly Dictionary<string, IView> _instances = new();
        private readonly CompositeDisposable _disposables = new();

        public ViewSynchronizer(IViewFactory viewFactory) => _viewFactory = viewFactory;

        public void SyncViews<TView, TViewModel>(
            IObservableCollection<TViewModel> source,
            TView prefab,
            Transform parent
        )
            where TViewModel : IEntityViewModel
            where TView : EntityView<TViewModel>
        {
            foreach (var viewModel in source)
            {
                var view = _viewFactory.Create(viewModel, prefab, parent);
                _instances[viewModel.ID] = view;
            }

            source.ObserveAdd()
                .Subscribe(addEvent =>
                {
                    var view = _viewFactory.Create(
                        addEvent.Value, prefab, parent);
                    _instances[addEvent.Value.ID] = view;
                })
                .AddTo(_disposables);

            source.ObserveRemove()
                .Subscribe(removeEvent =>
                {
                    if (_instances.TryGetValue(removeEvent.Value.ID, out var view) &&
                        view is MonoBehaviour monoBehaviour)
                    {
                        Object.Destroy(monoBehaviour.gameObject);
                        _instances.Remove(removeEvent.Value.ID);
                    }

                    removeEvent.Value.Dispose();
                })
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            foreach (var view in _instances.Values)
            {
                if (view is MonoBehaviour monoBehaviour)
                    Object.Destroy(monoBehaviour.gameObject);
            }

            _instances.Clear();
            _disposables.Dispose();
        }
    }


    public interface IViewSynchronizer
    {
        void SyncViews<TView, TViewModel>(
            IObservableCollection<TViewModel> source,
            TView prefab,
            Transform parent)
            where TViewModel : IEntityViewModel
            where TView : EntityView<TViewModel>;
    }
}