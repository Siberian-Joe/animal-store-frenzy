using System;
using R3;
using UnityEngine;
using IViewModel = NewCore.ViewModels.IViewModel;

namespace NewCore.Views
{
    public interface IView
    {
    }

    public abstract class View<TViewModel> : MonoBehaviour, IView, IDisposable
        where TViewModel : IViewModel
    {
        protected readonly CompositeDisposable Disposables = new();

        protected TViewModel ViewModel { get; private set; }

        public void Bind(TViewModel viewModel)
        {
            ViewModel = viewModel;
            OnBind();
        }

        protected virtual void OnBind()
        {
        }

        protected virtual void OnDestroy() => Dispose();

        public virtual void Dispose() => Disposables?.Dispose();
    }
}