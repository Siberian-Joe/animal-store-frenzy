using NewCore.Domain;
using R3;
using UnityEngine;

namespace NewCore.Data
{
    public abstract class NavigableEntity<TModel> : Entity<TModel>, INavigableEntityProxy
        where TModel : NavigableEntityData
    {
        public ReactiveProperty<Vector3> TargetPosition { get; private set; }

        public override void Initialize(TModel data)
        {
            base.Initialize(data);
            TargetPosition = new ReactiveProperty<Vector3>(data.Position);
            TargetPosition
                .Skip(1)
                .Subscribe(position => Position.Value = position)
                .AddTo(Disposables);
        }
    }
}