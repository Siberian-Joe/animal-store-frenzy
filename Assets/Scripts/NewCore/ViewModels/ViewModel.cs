using NewCore.Data;
using NewCore.Domain;
using R3;

namespace NewCore.ViewModels
{
    public abstract class ViewModel : IViewModel
    {
        protected readonly CompositeDisposable Disposables = new();

        public virtual void Dispose() => Disposables?.Dispose();
    }

    public abstract class ViewModel<TModel, TProxy> : ViewModel where TProxy : Proxy<TModel> where TModel : IModel
    {
        public TProxy Proxy { get; }

        protected ViewModel(TProxy proxy) => Proxy = proxy;

        public override void Dispose() => Proxy?.Dispose();
    }
}