namespace Interfaces.Core
{
    public interface IViewModel<out TModel> where TModel : IModel
    {
        TModel Model { get; }
    }
}