using NewCore.Domain;

namespace NewCore.Data
{
    public interface IEntityProxy<in TModel> : IProxy<TModel> where TModel : Entity
    {
        string Id { get; }
    }
}