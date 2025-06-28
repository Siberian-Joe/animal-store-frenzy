using NewCore.Data;
using NewCore.ViewModels;
using Zenject;

namespace NewCore.Factories
{
    public class ViewModelFactory : IViewModelFactory
    {
        private readonly DiContainer _container;

        public ViewModelFactory(DiContainer container) => _container = container;

        public TViewModel Create<TViewModel>()
            where TViewModel : IViewModel =>
            _container.Instantiate<TViewModel>();

        public TViewModel Create<TProxy, TViewModel>(TProxy proxy)
            where TProxy : IProxy
            where TViewModel : IViewModel =>
            _container.Instantiate<TViewModel>(new object[] { proxy });
    }
}