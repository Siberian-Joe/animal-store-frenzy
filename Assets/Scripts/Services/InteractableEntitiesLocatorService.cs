using System.Collections.Generic;
using Core.Enums;
using Interfaces.Core;
using Interfaces.Interactions;
using Interfaces.Services;
using UnityEngine;

namespace Services
{
    public class InteractableEntitiesLocatorService : IInteractableEntitiesLocatorService
    {
        private readonly Dictionary<InteractableEntityType, List<IInteractableEntityViewModel>> _entities = new();
        private readonly ILoggingService _loggingService;

        public InteractableEntitiesLocatorService(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public void RegisterEntity(IInteractableEntityViewModel interactableEntityViewModel)
        {
            if (interactableEntityViewModel.Transform.Value == null)
            {
                _loggingService.LogError("Transform is null");
                return;
            }

            var entityType = interactableEntityViewModel.InteractableEntityType.Value;
            if (_entities.ContainsKey(entityType) == false)
            {
                _entities[entityType] = new List<IInteractableEntityViewModel>();
            }

            _entities[entityType].Add(interactableEntityViewModel);
        }

        public void UnregisterEntity(IInteractableEntityViewModel interactableEntityViewModel)
        {
            var entityType = interactableEntityViewModel.InteractableEntityType.Value;
            if (_entities.TryGetValue(entityType, out var entityList) == false)
            {
                _loggingService.LogError($"No entities of type {entityType} registered");
                return;
            }

            entityList.Remove(interactableEntityViewModel);
        }

        public Vector2 FindPositionNearestObjectByType(InteractableEntityType entityType, Vector2 customerPosition)
        {
            if (_entities.TryGetValue(entityType, out var entityList) == false || entityList.Count == 0)
            {
                _loggingService.LogError($"No entities of type {entityType} registered");
                return Vector2.zero;
            }

            return FindNearestEntityPosition(entityList, customerPosition);
        }

        public IInteraction FindNearestObjectByType<TInteraction>(InteractableEntityType entityType,
            Vector2 customerPosition)
            where TInteraction : class, IInteraction
        {
            if (_entities.TryGetValue(entityType, out var entityList) == false || entityList.Count == 0)
            {
                _loggingService.LogError($"No entities of type {entityType} registered");
                return default;
            }

            var nearestEntity = FindNearestEntity(entityList, customerPosition);
            return nearestEntity as TInteraction;
        }

        //TODO: Refactor to use spatial hash
        private IInteractableEntityViewModel FindNearestEntity(IEnumerable<IInteractableEntityViewModel> entities,
            Vector2 customerPosition)
        {
            IInteractableEntityViewModel nearestEntity = null;
            var nearestDistance = float.MaxValue;

            foreach (var entity in entities)
            {
                if (entity.Transform.Value == null)
                {
                    continue;
                }

                var distance = Vector2.Distance(customerPosition, entity.Transform.Value.position);
                if (distance < nearestDistance)
                {
                    nearestEntity = entity;
                    nearestDistance = distance;
                }
            }

            return nearestEntity;
        }

        private Vector2 FindNearestEntityPosition(IEnumerable<IInteractableEntityViewModel> entities,
            Vector2 customerPosition)
        {
            var nearestEntity = FindNearestEntity(entities, customerPosition);
            return nearestEntity?.Transform.Value?.position ?? Vector2.zero;
        }
    }
}