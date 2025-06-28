using NewCore.Data;
using R3;

namespace NewCore.ViewModels
{
    public abstract class ViewModel : IViewModel
    {
        protected readonly CompositeDisposable Disposables = new();

        public virtual void Dispose() => Disposables?.Dispose();
    }

    public abstract class ViewModel<TProxy> : ViewModel
        where TProxy : IProxy
    {
        public TProxy Proxy { get; }

        protected ViewModel(TProxy proxy) => Proxy = proxy;

        public override void Dispose()
        {
            base.Dispose();
            Proxy?.Dispose();
        }
    }
}