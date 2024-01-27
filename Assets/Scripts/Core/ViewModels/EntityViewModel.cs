using Core.Models;
using Interfaces.Core;
using Interfaces.Services.DataServices;
using UniRx;
using UnityEngine;

namespace Core.ViewModels
{
    public abstract class EntityViewModel<TModel> : ViewModel<TModel>, IEntityViewModel where TModel : EntityModel
    {
        public IReadOnlyReactiveProperty<Transform> Transform => Model.Transform;

        protected EntityViewModel(IDataService dataService) : base(dataService)
        {
            Observable.EveryUpdate().Subscribe(_ => Update()).AddTo(Disposable);
            Observable.EveryFixedUpdate().Subscribe(_ => FixedUpdate()).AddTo(Disposable);
        }

        protected virtual void Update()
        {
        }
        
        protected virtual void FixedUpdate()
        {
        }

        public void SetTransform(Transform transform)
        {
            Model.Transform.Value = transform;
        }
    }
}