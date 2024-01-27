using Interfaces.Core;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.Views
{
    public class View<TViewModel> : MonoBehaviour, IView where TViewModel : IViewModel
    {
        protected TViewModel ViewModel { get; private set; }
        protected CompositeDisposable Disposable { get; } = new();

        [Inject]
        private void Constructor(TViewModel viewModel)
        {
            ViewModel = viewModel;
            Initialize();
        }

        protected virtual void Initialize()
        {
        }

        protected virtual void OnDestroy()
        {
            Dispose();
        }

        public virtual void Dispose()
        {
            foreach (var disposable in Disposable)
                disposable.Dispose();

            Disposable.Clear();
            ViewModel.Dispose();
        }
    }
}