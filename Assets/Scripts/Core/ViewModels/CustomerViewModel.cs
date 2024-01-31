using Core.Enums;
using Interfaces.Services;
using Interfaces.Services.DataServices;
using UnityEngine;

namespace Core.ViewModels
{
    public class CustomerViewModel : MovableEntityViewModel<CustomerModel>
    {
        private readonly IInteractableEntitiesLocatorService _interactableEntitiesLocatorService;

        public CustomerViewModel(IDataService dataService,
            IInteractableEntitiesLocatorService interactableEntitiesLocatorService) : base(dataService)
        {
            _interactableEntitiesLocatorService = interactableEntitiesLocatorService;
        }

        //TODO: Extract to a common implementation for automatic determination of the type of walking animation
        public int CalculateDirectionIndex(Vector2 direction)
        {
            return direction.x < 0 - 0.1f ? 3 : direction.x > 0 + 0.1f ? 2 : direction.y > 0 + 0.1f ? 1 : 0;
        }

        public void UpdateTargetPosition(Vector2 position)
        {
            Model.TargetPosition.Value = position;
        }

        protected override void FixedUpdate()
        {
            if (TryMoveToPosition(TargetPosition.Value))
                base.FixedUpdate();
            else
                StopMoving();
        }

        protected override CustomerModel CreateDefaultModel()
        {
            return new CustomerModel();
        }

        private bool TryMoveToPosition(Vector2 position)
        {
            if ((position - (Vector2)Transform.Value.position).sqrMagnitude < 0.1f * 0.1f)
                return false;

            UpdateDirection(position);

            return true;
        }

        private void StopMoving()
        {
            UpdateDirection(Vector2.zero);
        }

        public void SetInteractableObjectType(InteractableEntityType interactableEntityType)
        {
            Model.TargetPosition.Value =
                _interactableEntitiesLocatorService
                    .FindPositionNearestObjectByType(interactableEntityType, Model.Transform.Value.position);
        }
    }
}