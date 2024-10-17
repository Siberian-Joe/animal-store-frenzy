using System;
using System.Collections.Generic;

namespace Services
{
    public class InteractiveObjectLocationService : IObjectRegistrationService, IObjectQueryService
    {
        private readonly Dictionary<Type, List<IInteractiveObject>> _locations = new();
        
        public void Register(IInteractiveObject interactiveObject)
        {
            var type = interactiveObject.GetType();
            if (_locations.ContainsKey(type) == false)
            {
                _locations[type] = new List<IInteractiveObject>();
            }

            _locations[type].Add(interactiveObject);
        }
        
        public void Unregister(IInteractiveObject interactiveObject)
        {
            var type = interactiveObject.GetType();
            if (_locations.ContainsKey(type))
            {
                _locations[type].Remove(interactiveObject);
                if (_locations[type].Count == 0)
                {
                    _locations.Remove(type);
                }
            }
        }

        public bool Has<T>() where T : IInteractiveObject
        {
            return _locations.ContainsKey(typeof(T));
        }

        public bool TryFindNearest<T>(IInteractiveObject from, out T nearest) where T : IInteractiveObject
        {
            nearest = default;
            if (_locations.TryGetValue(typeof(T), out var interactiveObjects) == false)
            {
                return false;
            }

            var nearestDistance = float.MaxValue;
            foreach (var interactiveObject in interactiveObjects)
            {
                var distance = (from.Position - interactiveObject.Position).sqrMagnitude;
                if (distance < nearestDistance)
                {
                    nearest = (T)interactiveObject;
                    nearestDistance = distance;
                }
            }

            return nearest != null;
        }
    }
}