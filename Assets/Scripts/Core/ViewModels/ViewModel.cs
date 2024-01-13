using System;
using System.Collections.Generic;
using Interfaces.Core;
using Interfaces.Services.DataServices;

namespace Core.ViewModels
{
    public abstract class ViewModel<TModel> : IViewModel where TModel : IModel
    {
        protected TModel Model { get; private set; }

        protected ICollection<IDisposable> Disposables { get; } = new List<IDisposable>();

        private readonly IDataService _dataService;

        protected ViewModel(IDataService dataService)
        {
            _dataService = dataService;
            Initialize();
        }

        protected abstract TModel CreateDefaultModel();

        protected virtual void Initialize()
        {
            Model = _dataService.Load(GetType().Name, CreateDefaultModel());
        }

        public virtual void Dispose()
        {
            foreach (var disposable in Disposables)
                disposable.Dispose();

            Disposables.Clear();
        }
    }
}