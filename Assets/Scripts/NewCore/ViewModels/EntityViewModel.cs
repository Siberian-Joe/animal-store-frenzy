using NewCore.Data;

namespace NewCore.ViewModels
{
    public abstract class EntityViewModel<TProxy> : ViewModel<TProxy>
        where TProxy : IEntityProxy
    {
        public string Id => Proxy.Id;

        protected EntityViewModel(TProxy proxy) : base(proxy) { }
    }
}