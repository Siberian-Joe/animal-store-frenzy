using NewCore.Data;
using NewCore.Domain;

namespace NewCore.ViewModels
{
    public abstract class EntityViewModel<TModel, TProxy> : ViewModel<TModel, TProxy>
        where TProxy : EntityProxy<TModel>
        where TModel : Entity
    {
        public string Id => Proxy.Id;

        protected EntityViewModel(TProxy proxy) : base(proxy)
        {
        }
    }
}