using System;
using NewCore.ViewModels;
using R3;
using UnityEngine;

namespace NewCore.Views
{
    public abstract class View<TViewModel> : MonoBehaviour, IDisposable where TViewModel : IViewModel
    {
        protected TViewModel ViewModel { get; private set; }
        protected readonly CompositeDisposable Disposables = new();

        public void Bind(TViewModel viewModel)
        {
            ViewModel = viewModel;
            OnBind();
        }

        protected virtual void OnBind()
        {
        }

        protected virtual void OnDestroy() => Dispose();

        public virtual void Dispose()
        {
            ViewModel?.Dispose();
            Disposables?.Dispose();
        }
    }
}