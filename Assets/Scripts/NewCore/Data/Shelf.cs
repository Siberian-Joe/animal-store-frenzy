using NewCore.Domain;
using R3;

namespace NewCore.Data
{
    public class Shelf : Entity<ShelfData>
    {
        public string Name { get; private set; }
        public ReactiveProperty<int> Capacity { get; private set; }
        public ReactiveProperty<int> Level { get; private set; }

        public override void Initialize(ShelfData model)
        {
            base.Initialize(model);

            Name = model.Name;
            Capacity = new ReactiveProperty<int>(model.Capacity);
            Level = new ReactiveProperty<int>(model.Level);

            Capacity
                .Skip(1)
                .Subscribe(capacity => model.Capacity = capacity);
            Level
                .Skip(1)
                .Subscribe(level => model.Level = level);
        }

        public override ShelfData ToModel()
        {
            return new ShelfData
            {
                Id = Id,
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