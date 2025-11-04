using NewCore.Data;
using R3;
using UnityEngine;

namespace NewCore.ViewModels.World
{
    public abstract class EntityViewModel<TProxy> : ViewModel<TProxy>, IEntityViewModel
        where TProxy : Proxy, IEntityProxy
    {
        public string Id => Proxy.Id;
        public ReactiveProperty<Vector3> Position => Proxy.Position;

        protected EntityViewModel(TProxy proxy) : base(proxy)
        {
        }
    }
}