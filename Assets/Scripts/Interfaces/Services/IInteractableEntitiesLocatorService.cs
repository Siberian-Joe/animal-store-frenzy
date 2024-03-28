using Core.Enums;
using Interfaces.Core;
using UnityEngine;

namespace Interfaces.Services
{
    public interface IInteractableEntitiesLocatorService
    {
        void RegisterEntity(IInteractableEntityViewModel interactableEntityViewModel);
        void UnregisterEntity(IInteractableEntityViewModel interactableEntityViewModel);

        Vector2 FindPositionNearestObjectByType(InteractableEntityType interactableEntityType,
            Vector2 customerPosition);
    }
}