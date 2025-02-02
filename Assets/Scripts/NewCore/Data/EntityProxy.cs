using NewCore.Domain;

namespace NewCore.Data
{
    public abstract class EntityProxy<TModel> : IProxy<TModel> where TModel : Entity
    {
        public string Id { get; private set; }

        public abstract TModel ToModel();
        public virtual void Initialize(TModel model) => Id = model.Id;

        public virtual void Dispose()
        {
        }
    }
}