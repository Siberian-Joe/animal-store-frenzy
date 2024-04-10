using Core.Enums;
using Core.Models;
using Interfaces.Interactions;
using Interfaces.Services;
using Interfaces.Services.DataServices;
using UnityEngine;
using UnityEngine.AI;

namespace Core.ViewModels
{
    public class CustomerViewModel : MovableAndInteractableEntityViewModel<CustomerModel>, IInteraction
    {
        private NavMeshAgent _agent = new();

        public CustomerViewModel(IDataService dataService, ILoggingService loggingService,
            IInteractableEntitiesLocatorService interactableEntitiesLocatorService) : base(dataService, loggingService,
            interactableEntitiesLocatorService)
        {
        }

        public override void Update()
        {
            base.Update();

            if (_agent == null)
                return;

            UpdateDirection(_agent.velocity);
        }

        //TODO: Extract to a common implementation for automatic determination of the type of walking animation

        public int CalculateDirectionIndex(Vector2 direction)
        {
            return direction.x < 0 - 0.1f ? 3 : direction.x > 0 + 0.1f ? 2 : direction.y > 0 + 0.1f ? 1 : 0;
        }

        public void SetInteractableObjectType(InteractableEntityType interactableEntityType)
        {
            Model.TargetPosition.Value =
                InteractableEntitiesLocatorService
                    .FindPositionNearestObjectByType(interactableEntityType, Transform.Value.position);
        }

        public void SetAgent(NavMeshAgent agent)
        {
            _agent = agent;
        }

        protected override CustomerModel CreateDefaultModel()
        {
            return new CustomerModel();
        }

        public void Interact()
        {
        }
    }
}