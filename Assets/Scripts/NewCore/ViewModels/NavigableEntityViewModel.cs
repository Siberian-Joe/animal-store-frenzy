using NewCore.Data;
using R3;
using UnityEngine;

namespace NewCore.ViewModels
{
    public abstract class NavigableEntityViewModel<TProxy> : EntityViewModel<TProxy>
        where TProxy : INavigableEntityProxy
    {
        public ReactiveProperty<Vector3> TargetPosition => Proxy.TargetPosition;

        protected NavigableEntityViewModel(TProxy proxy) : base(proxy)
        {
        }
        
        public override void Dispose()
        {
            base.Dispose();
            TargetPosition?.Dispose();
        }
    }
}