using NewCore.ViewBinders;
using NewCore.ViewModels;
using R3;
using UnityEngine;
using Zenject;

namespace NewCore.Views.World
{
    public class WorldView : NodeView<WorldViewModel>
    {
        [SerializeField] private PlayerView _playerViewPrefab;

        [Inject] private IViewBinder _binder;

        private PlayerView _playerViewInstance;

        protected override void OnBind()
        {
            base.OnBind();

            ViewModel.Player?
                .Where(player => player != null)
                .Subscribe(viewModel =>
                {
                    _binder.BindSingle(
                        viewModel,
                        _playerViewPrefab,
                        transform);
                })
                .AddTo(Disposables);
        }
    }
}