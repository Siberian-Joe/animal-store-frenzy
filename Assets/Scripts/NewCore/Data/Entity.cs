using NewCore.Domain;
using R3;
using UnityEngine;

namespace NewCore.Data
{
    public abstract class Entity<TModel> : Proxy<TModel>, IEntityProxy
        where TModel : EntityData
    {
        public string ID { get; private set; }
        public ReactiveProperty<Vector3> Position { get; private set; }

        public override void Initialize(TModel data)
        {
            ID = data.ID;
            Position = new ReactiveProperty<Vector3>(data.Position);
            Position
                .Skip(1)
                .Subscribe(position => data.Position = position)
                .AddTo(Disposables);
        }
    }
}