using NewCore.Domain;
using R3;
using UnityEngine;

namespace NewCore.Data
{
    public class Customer : Entity<CustomerData>
    {
        public string Type;
        public ReactiveProperty<Vector2> Position { get; private set; }

        public override void Initialize(CustomerData data)
        {
            base.Initialize(data);

            Type = data.Type;
            Position = new ReactiveProperty<Vector2>(data.Position);

            Position
                .Skip(1)
                .Subscribe(position => data.Position = position);
        }

        public override CustomerData ToModel()
        {
            return new CustomerData
            {
                ID = ID,
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