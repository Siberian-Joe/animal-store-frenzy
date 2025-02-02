namespace NewCore.ViewModels
{
    public interface IViewModelFactory
    {
        TViewModel Create<TProxy, TViewModel>(TProxy proxy)
            where TViewModel : IViewModel;
    }
}