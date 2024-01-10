namespace Interfaces.Core
{
    public interface IView<out TViewModel> where TViewModel : IViewModel<IModel>
    {
        TViewModel ViewModel { get; }
    }
}