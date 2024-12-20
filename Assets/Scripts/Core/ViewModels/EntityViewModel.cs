using Core.Models;
using Interfaces.Core;
using Interfaces.Services.DataServices;
using R3;
using UnityEngine;

namespace Core.ViewModels
{
    public abstract class EntityViewModel<TModel> : ViewModel<TModel>, IEntityViewModel where TModel : EntityModel
    {
        public ReadOnlyReactiveProperty<Transform> Transform => Model.Transform;

        protected EntityViewModel(IDataService dataService) : base(dataService)
        {
        }

        public void SetTransform(Transform transform)
        {
            Model.Transform.Value = transform;
        }
    }
}