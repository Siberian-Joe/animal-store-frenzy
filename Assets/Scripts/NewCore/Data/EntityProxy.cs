using NewCore.Domain;

namespace NewCore.Data
{
    public abstract class EntityProxy<TModel> : Proxy<TModel> where TModel : Entity
    {
        public string Id { get; private set; }
        public override void Initialize(TModel model) => Id = model.Id;
    }
}