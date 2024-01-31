using Interfaces.Core;
using Interfaces.Services.DataServices;
using UniRx;

namespace Core.ViewModels
{
    public abstract class ViewModel<TModel> : IViewModel where TModel : IModel
    {
        protected TModel Model { get; private set; }
        protected CompositeDisposable Disposable { get; } = new();

        protected ViewModel(IDataService dataService)
        {
            Model = dataService.Load(GetType().Name, CreateDefaultModel());
        }

        protected abstract TModel CreateDefaultModel();

        public virtual void Dispose()
        {
            foreach (var disposable in Disposable)
                disposable.Dispose();

            Disposable.Clear();
        }
    }
}