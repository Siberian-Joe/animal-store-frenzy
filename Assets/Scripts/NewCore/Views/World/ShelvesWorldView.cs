using NewCore.Factories;
using NewCore.ViewBinders;
using NewCore.ViewModels.World;
using UnityEngine;
using Zenject;

namespace NewCore.Views.World
{
    public class ShelvesWorldView : NodeView<ShelvesWorldViewModel>
    {
        [SerializeField] private ShelfView _shelfViewPrefab;

        [Inject] private IViewFactory _viewFactory;
        [Inject] private IViewBinder _viewBinder;

        protected override void OnBind() => _viewBinder.Bind(ViewModel.Shelves, _shelfViewPrefab, transform);
    }
}