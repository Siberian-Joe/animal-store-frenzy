using NewCore.ViewModels;
using Zenject;

namespace NewCore.Factories
{
    public class ViewModelFactory : IViewModelFactory
    {
        private readonly DiContainer _container;

        public ViewModelFactory(DiContainer container)
        {
            _container = container;
        }

        public TViewModel Create<TProxy, TViewModel>(TProxy proxy)
            where TViewModel : IViewModel
        {
            return _container.Instantiate<TViewModel>(new object[] { proxy });
        }
    }
}