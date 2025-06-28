using NewCore.Data;
using NewCore.ViewModels;

namespace NewCore.Factories
{
    public interface IViewModelFactory
    {
        TViewModel Create<TViewModel>()
            where TViewModel : IViewModel;

        TViewModel Create<TProxy, TViewModel>(TProxy proxy)
            where TProxy : IProxy
            where TViewModel : IViewModel;
    }
}