using R3;

namespace NewCore.Data
{
    public class Proxy : IProxy
    {
        protected CompositeDisposable Disposables = new();

        public virtual void Dispose()
        {
            Disposables.Dispose();
        }
    }
}