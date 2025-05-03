using NewCore.Domain;
using R3;

namespace NewCore.Data
{
    public class Proxy : IProxy
    {
        protected readonly CompositeDisposable Disposables = new();

        public virtual void Dispose() => Disposables.Dispose();
    }

    public abstract class Proxy<TModel> : Proxy where TModel : IModel
    {
        public abstract void Initialize(TModel model);
        public abstract TModel ToModel();
    }
}