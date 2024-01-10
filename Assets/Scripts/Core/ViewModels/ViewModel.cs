using System;
using System.Collections.Generic;
using Interfaces.Core;
using Interfaces.Services.DataServices;

namespace Core.ViewModels
{
    public abstract class ViewModel<TModel> : IViewModel<TModel> where TModel : IModel
    {
        public TModel Model { get; private set; }
    
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
    }
}