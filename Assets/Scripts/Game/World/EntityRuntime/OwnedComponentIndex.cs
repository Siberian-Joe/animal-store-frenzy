using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.World.EntityRuntime
{
    public sealed class OwnedComponentIndex
    {
        private readonly Dictionary<Type, Component> _firstByType = new();
        private readonly Dictionary<Type, List<Component>> _allByType = new();

        private OwnedComponentIndex()
        {
        }

        public static OwnedComponentIndex Build(EntityRoot ownerRoot)
        {
            if (ownerRoot == false)
                throw new ArgumentNullException(nameof(ownerRoot));

            var index = new OwnedComponentIndex();
            index.Rebuild(ownerRoot);
            return index;
        }

        public bool TryGet<TComponent>(out TComponent component)
            where TComponent : class
        {
            if (_firstByType.TryGetValue(typeof(TComponent), out var raw) &&
                raw is TComponent typed)
            {
                component = typed;
                return true;
            }

            component = null;
            return false;
        }

        public bool TryGet<TComponent>(
            Component scope,
            out TComponent component,
            Component excluded = null)
            where TComponent : class
        {
            component = null;

            if (scope == false)
                return false;

            if (_allByType.TryGetValue(typeof(TComponent), out var candidates) == false)
                return false;

            var scopeTransform = scope.transform;

            foreach (var candidate in candidates)
            {
                if (candidate == false || candidate == excluded)
                    continue;

                if (candidate.transform.IsChildOf(scopeTransform) == false)
                    continue;

                if (candidate is not TComponent typed)
                    continue;

                component = typed;
                return true;
            }

            return false;
        }

        public void Collect<TComponent>(
            Component scope,
            List<TComponent> results,
            Component excluded = null)
            where TComponent : class
        {
            if (results == null)
                throw new ArgumentNullException(nameof(results));

            if (scope == false)
                return;

            if (_allByType.TryGetValue(typeof(TComponent), out var candidates) == false)
                return;

            var scopeTransform = scope.transform;

            foreach (var candidate in candidates)
            {
                if (candidate == false || candidate == excluded)
                    continue;

                if (candidate.transform.IsChildOf(scopeTransform) == false)
                    continue;

                if (candidate is not TComponent typed)
                    continue;

                results.Add(typed);
            }
        }

        private void Rebuild(EntityRoot ownerRoot)
        {
            _firstByType.Clear();
            _allByType.Clear();

            var components = ownerRoot.GetComponentsInChildren<Component>(true);

            foreach (var component in components)
            {
                if (component == false)
                    continue;

                if (component.GetComponentInParent<EntityRoot>() != ownerRoot)
                    continue;

                RegisterComponent(component);
            }
        }

        private void RegisterComponent(Component component)
        {
            var concreteType = component.GetType();
            RegisterType(concreteType, component);

            var baseType = concreteType.BaseType;
            while (baseType != null && typeof(Component).IsAssignableFrom(baseType))
            {
                RegisterType(baseType, component);
                baseType = baseType.BaseType;
            }

            var interfaces = concreteType.GetInterfaces();
            foreach (var typedInterface in interfaces)
                RegisterType(typedInterface, component);
        }

        private void RegisterType(Type type, Component component)
        {
            _firstByType.TryAdd(type, component);

            if (_allByType.TryGetValue(type, out var list) == false)
            {
                list = new List<Component>(2);
                _allByType.Add(type, list);
            }

            list.Add(component);
        }
    }
}