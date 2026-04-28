using System;
using System.Collections.Generic;
using Game.World.EntityRuntime;
using UnityEngine;

namespace Game.World.Interactions
{
    [DisallowMultipleComponent]
    public sealed class InteractionActorPart : EntityComponent, IInteractionActor
    {
        private Dictionary<Type, IInteractionRole> _rolesByType;

        public override int ActivationOrder => 360;

        protected override void OnActivate() => EnsureCached();

        public bool TryGetRole<TRole>(out TRole role)
            where TRole : class, IInteractionRole
        {
            EnsureCached();

            if (_rolesByType.TryGetValue(typeof(TRole), out var registeredRole) &&
                registeredRole is TRole typedRole)
            {
                role = typedRole;
                return true;
            }

            role = null;
            return false;
        }

        private void EnsureCached()
        {
            if (_rolesByType != null)
                return;

            if (OwnerRoot == false)
            {
                throw new InvalidOperationException(
                    $"{nameof(InteractionActorPart)} on '{name}' must live under an {nameof(EntityRoot)}.");
            }

            var rolesByType = new Dictionary<Type, IInteractionRole>();
            var registry = new InteractionRoleRegistry(rolesByType, OwnerRoot.name);
            var roleProviders = new List<IInteractionRoleProvider>(4);
            OwnerRoot.CollectOwnedComponents(roleProviders);

            foreach (var roleProvider in roleProviders)
                roleProvider.RegisterRoles(registry);

            _rolesByType = rolesByType;
        }
    }
}