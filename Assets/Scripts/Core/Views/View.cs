using System;
using System.Collections.Generic;
using Interfaces.Core;
using UnityEngine;
using Zenject;

namespace Core.Views
{
    public class View<TViewModel> : MonoBehaviour, IView where TViewModel : IViewModel
    {
        protected TViewModel ViewModel { get; private set; }

        protected ICollection<IDisposable> Disposables { get; } = new List<IDisposable>();

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
            foreach (var disposable in Disposables)
                disposable.Dispose();

            Disposables.Clear();
            ViewModel.Dispose();
        }
    }
}