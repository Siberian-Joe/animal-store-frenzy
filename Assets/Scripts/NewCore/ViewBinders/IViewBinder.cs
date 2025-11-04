using NewCore.ViewModels.World;
using NewCore.Views;
using ObservableCollections;
using UnityEngine;

namespace NewCore.ViewBinders
{
    public interface IViewBinder
    {
        void Bind<TViewModel, TView>(
            IObservableCollection<TViewModel> source,
            TView prefab,
            Transform parent)
            where TViewModel : IEntityViewModel
            where TView : EntityView<TViewModel>;

        TView BindSingle<TViewModel, TView>(
            TViewModel viewModel,
            TView prefab,
            Transform parent)
            where TViewModel : IEntityViewModel
            where TView : EntityView<TViewModel>;
    }
}