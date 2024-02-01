using Core.Enums;
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
        }

        public void SetTransform(Transform transform)
        {
            Model.Transform.Value = transform;
        }
    }
}