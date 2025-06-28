using NewCore.Data;

namespace NewCore.ViewModels
{
    public abstract class EntityViewModel<TProxy> : ViewModel<TProxy>, IEntityViewModel
        where TProxy : IEntityProxy
    {
        public string ID => Proxy.ID;

        protected EntityViewModel(TProxy proxy) : base(proxy)
        {
        }
    }
}