using NewCore.Data;
using R3;
using UnityEngine;

namespace NewCore.ViewModels
{
    public abstract class EntityViewModel<TProxy> : ViewModel<TProxy>, IEntityViewModel
        where TProxy : IEntityProxy
    {
        public string ID => Proxy.ID;
        public ReactiveProperty<Vector3> Position => Proxy.Position;

        protected EntityViewModel(TProxy proxy) : base(proxy)
        {
        }
    }
}