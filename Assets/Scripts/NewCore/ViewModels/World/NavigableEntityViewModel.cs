using NewCore.Data;
using R3;
using UnityEngine;

namespace NewCore.ViewModels.World
{
    public abstract class NavigableEntityViewModel<TProxy> : EntityViewModel<TProxy>
        where TProxy : Proxy, INavigableEntityProxy
    {
        public ReactiveProperty<Vector3> TargetPosition => Proxy.TargetPosition;
        public Observable<Unit> Arrived => _arrived;

        private readonly Subject<Unit> _arrived = new();

        protected NavigableEntityViewModel(TProxy proxy) : base(proxy)
        {
        }
        
        public void NotifyArrived() => _arrived.OnNext(Unit.Default);

        public override void Dispose()
        {
            base.Dispose();
            TargetPosition?.Dispose();
            _arrived?.Dispose();
        }
    }
}