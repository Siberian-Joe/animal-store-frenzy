using System;
using System.Collections.Generic;
using Interfaces.Core;
using UnityEngine;
using Zenject;

namespace Core.Views
{
    public class View<TViewModel> : MonoBehaviour, IView<TViewModel> where TViewModel : IViewModel<IModel>
    {
        public TViewModel ViewModel { get; private set; }
    
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
            foreach (var disposable in Disposables)
            {
                disposable.Dispose();
            }
        }
    }
}