using NewCore.Domain;
using R3;

namespace NewCore.Data
{
    public class Shelf : Entity<ShelfData>
    {
        public string Name { get; private set; }
        public ReactiveProperty<int> Capacity { get; private set; }
        public ReactiveProperty<int> Level { get; private set; }

        public override void Initialize(ShelfData data)
        {
            base.Initialize(data);

            Name = data.Name;
            Capacity = new ReactiveProperty<int>(data.Capacity);
            Level = new ReactiveProperty<int>(data.Level);

            Capacity
                .Skip(1)
                .Subscribe(capacity => data.Capacity = capacity);
            Level
                .Skip(1)
                .Subscribe(level => data.Level = level);
        }

        public override ShelfData ToModel()
        {
            return new ShelfData
            {
                ID = ID,
                Name = Name,
                Capacity = Capacity.Value,
                Level = Level.Value
            };
        }

        public override void Dispose()
        {
            base.Dispose();

            Capacity.Dispose();
            Level.Dispose();
        }
    }
}