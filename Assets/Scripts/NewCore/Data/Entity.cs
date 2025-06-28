using NewCore.Domain;

namespace NewCore.Data
{
    public abstract class Entity<TModel> : Proxy<TModel>, IEntityProxy
        where TModel : EntityData
    {
        public string ID { get; private set; }
        public override void Initialize(TModel data) => ID = data.ID;
    }
}