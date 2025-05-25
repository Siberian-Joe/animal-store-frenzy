using NewCore.Domain;

namespace NewCore.Data
{
    public abstract class Entity<TModel> : Proxy<TModel>, IEntityProxy
        where TModel : EntityData
    {
        public string Id { get; private set; }
        public override void Initialize(TModel model) => Id = model.Id;
    }
}