using NewCore.ViewModels;
using NewCore.ViewModels.World;
using NewCore.Views;
using UnityEngine;
using Zenject;

namespace NewCore.Factories
{
    public class ViewFactory : IViewFactory
    {
        private readonly DiContainer _container;

        public ViewFactory(DiContainer container) => _container = container;

        public TView Create<TView, TViewModel>(TViewModel viewModel, TView prefab, Transform parent)
            where TViewModel : IEntityViewModel
            where TView : EntityView<TViewModel>
        {
            if (prefab == null)
            {
                Debug.LogError($"Prefab for {typeof(TView)} is null");
                return null;
            }

            var view = _container.InstantiatePrefabForComponent<TView>(prefab, parent);
            view.Bind(viewModel);

            return view;
        }
    }
}