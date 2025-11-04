using NewCore.Domain;
using R3;
using UnityEngine;

namespace NewCore.Data
{
    public abstract class NavigableEntity<TModel> : Entity<TModel>, INavigableEntityProxy
        where TModel : NavigableEntityData
    {
        public ReactiveProperty<Vector3> TargetPosition { get; }

        protected NavigableEntity(TModel model) : base(model)
        {
            TargetPosition = new ReactiveProperty<Vector3>(model.Position);
            TargetPosition
                .Skip(1)
                .Subscribe(position => Position.Value = position)
                .AddTo(Disposables);
        }
    }
}