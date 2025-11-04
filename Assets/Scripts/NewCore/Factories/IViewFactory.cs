using NewCore.ViewModels;
using NewCore.ViewModels.World;
using NewCore.Views;
using UnityEngine;

namespace NewCore.Factories
{
    public interface IViewFactory
    {
        public TView Create<TView, TViewModel>(TViewModel viewModel, TView prefab, Transform parent)
            where TViewModel : IEntityViewModel
            where TView : EntityView<TViewModel>;
    }
}