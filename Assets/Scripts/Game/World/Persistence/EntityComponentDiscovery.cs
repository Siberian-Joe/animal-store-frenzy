using System;
using System.Collections.Generic;
using Game.World.EntityRuntime;
using UnityEngine;

namespace Game.World.Persistence
{
    public sealed class EntityComponentDiscovery : IEntityComponentDiscovery
    {
        public IReadOnlyList<IEntityComponent> Discover(EntityRoot root)
        {
            if (root == false)
                throw new ArgumentNullException(nameof(root));

            var discovered = root.GetComponentsInChildren<EntityComponent>(true);
            var components = new List<IEntityComponent>(discovered.Length);

            foreach (var component in discovered)
            {
                if (component == false)
                    continue;

                if (component.OwnerRoot != root)
                    continue;

                components.Add(component);
            }

            components.Sort(static (left, right) =>
            {
                var orderComparison = left.ActivationOrder.CompareTo(right.ActivationOrder);
                if (orderComparison != 0)
                    return orderComparison;

                var leftBehaviour = left as MonoBehaviour;
                var rightBehaviour = right as MonoBehaviour;

                return string.CompareOrdinal(
                    leftBehaviour ? leftBehaviour.GetType().Name : left.GetType().Name,
                    rightBehaviour ? rightBehaviour.GetType().Name : right.GetType().Name);
            });

            return components;
        }
    }
}