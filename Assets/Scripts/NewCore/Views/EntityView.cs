using NewCore.Components;
using NewCore.ViewModels;
using UnityEngine;

namespace NewCore.Views
{
    [RequireComponent(typeof(EntityIdentifier))]
    public abstract class EntityView<TViewModel> : View<TViewModel> where TViewModel : IEntityViewModel
    {
        public string ID => ViewModel?.ID ?? (_identifier ??= GetComponent<EntityIdentifier>()).ID;

        private EntityIdentifier _identifier;

        protected override void OnBind()
        {
            base.OnBind();

            if (_identifier == null)
                _identifier = GetComponent<EntityIdentifier>();

            _identifier.ID = ViewModel.ID;
        }
    }
}