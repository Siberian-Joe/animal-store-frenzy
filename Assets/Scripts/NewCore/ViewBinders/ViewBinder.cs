using System;
using NewCore.ViewModels;
using NewCore.Views;
using ObservableCollections;
using R3;
using UnityEngine;
using Zenject;

namespace NewCore.ViewBinders
{
    public class ViewBinder : IViewBinder, IDisposable
    {
        private readonly DiContainer _container;
        private readonly CompositeDisposable _disposables = new();

        public ViewBinder(DiContainer container) => _container = container;

        public void Bind<TViewModel, TView>(
            IObservableCollection<TViewModel> source,
            TView prefab,
            Transform parent)
            where TViewModel : IEntityViewModel
            where TView : EntityView<TViewModel>
        {
            foreach (var viewModel in source)
                BindSingle(viewModel, prefab, parent);

            source.ObserveAdd()
                .Subscribe(addEvent => BindSingle(addEvent.Value, prefab, parent))
                .AddTo(_disposables);

            source.ObserveRemove()
                .Subscribe(removeEvent => removeEvent.Value.Dispose())
                .AddTo(_disposables);
        }

        public TView BindSingle<TViewModel, TView>(
            TViewModel viewModel,
            TView prefab,
            Transform parent)
            where TViewModel : IEntityViewModel
            where TView : EntityView<TViewModel>
        {
            if (prefab == null)
                return null;
            
            var view = _container.InstantiatePrefabForComponent<TView>(prefab, parent);
            view.Bind(viewModel);
            return view;
        }

        public void Dispose() => _disposables.Dispose();
    }
}