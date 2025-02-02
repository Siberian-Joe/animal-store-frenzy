using NewCore.ViewModels;

namespace NewCore.Factories
{
    public interface IViewModelFactory
    {
        TViewModel Create<TProxy, TViewModel>(TProxy proxy)
            where TViewModel : IViewModel;
    }
}