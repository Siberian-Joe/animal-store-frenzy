using System;
using NewCore.Domain;
using R3;
using UnityEngine;

namespace NewCore.Data
{
    [Serializable]
    public class Customer : Entity<CustomerData>
    {
        public string Type;
        public ReactiveProperty<Vector3Int> Position { get; private set; }

        public override void Initialize(CustomerData model)
        {
            base.Initialize(model);

            Type = model.Type;
            Position = new ReactiveProperty<Vector3Int>(model.Position);

            Position.Skip(1).Subscribe(position => model.Position = position);
        }

        public override CustomerData ToModel()
        {
            return new CustomerData
            {
                Id = Id,
                Type = Type,
                Position = Position.Value
            };
        }

        public override void Dispose()
        {
            base.Dispose();

            Position.Dispose();
        }
    }
}