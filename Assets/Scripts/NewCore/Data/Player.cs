using NewCore.Domain;
using R3;
using UnityEngine;

namespace NewCore.Data
{
    public class Player : Entity<PlayerData>
    {
        public ReactiveProperty<Vector2> Position { get; private set; }
        public ReactiveProperty<Vector2> TargetPosition { get; private set; }

        public override void Initialize(PlayerData data)
        {
            base.Initialize(data);
            Position = new ReactiveProperty<Vector2>(data.Position);
            Position
                .Skip(1)
                .Subscribe(position => data.Position = position);

            TargetPosition = new ReactiveProperty<Vector2>(Position.Value);
            TargetPosition
                .Skip(1)
                .Subscribe(position => Position.Value = position);
        }

        public override PlayerData ToModel()
        {
            return new PlayerData
            {
                ID = ID,
                Position = Position.Value
            };
        }

        public override void Dispose()
        {
            base.Dispose();
            Position.Dispose();
            TargetPosition.Dispose();
        }
    }
}