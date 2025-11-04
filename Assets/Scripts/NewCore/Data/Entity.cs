using System;
using NewCore.Domain;
using R3;
using UnityEngine;

namespace NewCore.Data
{
    public abstract class Entity<TModel> : Proxy<TModel>, IEntityProxy, IEquatable<Entity<TModel>>, IEquatable<TModel>
        where TModel : EntityData
    {
        public string Id { get; }
        public ReactiveProperty<Vector3> Position { get; }

        protected Entity(TModel model) : base(model)
        {
            Id = model.Id;
            Position = new ReactiveProperty<Vector3>(model.Position);
            Position
                .Take(1)
                .Subscribe(position => model.Position = position)
                .AddTo(Disposables);
        }

        public bool Equals(Entity<TModel> other) =>
            !ReferenceEquals(other, null) && Id == other.Id;

        public bool Equals(TModel other) =>
            !ReferenceEquals(other, null) && Id == other.Id;

        public override bool Equals(object obj) =>
            obj switch
            {
                Entity<TModel> entity => Equals(entity),
                TModel model => Equals(model),
                _ => false
            };

        public override int GetHashCode() => Id != null ? Id.GetHashCode() : 0;

        public static bool operator ==(Entity<TModel> a, Entity<TModel> b) =>
            ReferenceEquals(a, b) || (a?.Equals(b) ?? false);

        public static bool operator !=(Entity<TModel> a, Entity<TModel> b) =>
            !(a == b);
    }
}