using System;
using System.Collections.Generic;

namespace Game.World.Interactions
{
    public interface IInteractionRoleProvider
    {
        void RegisterRoles(InteractionRoleRegistry registry);
    }

    public sealed class InteractionRoleRegistry
    {
        private readonly Dictionary<Type, IInteractionRole> _rolesByType;
        private readonly string _ownerName;

        public InteractionRoleRegistry(
            Dictionary<Type, IInteractionRole> rolesByType,
            string ownerName)
        {
            _rolesByType = rolesByType ?? throw new ArgumentNullException(nameof(rolesByType));
            _ownerName = ownerName ?? throw new ArgumentNullException(nameof(ownerName));
        }

        public void Add<TRole>(TRole role)
            where TRole : class, IInteractionRole
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            var roleType = typeof(TRole);

            if (_rolesByType.TryGetValue(roleType, out var existingRole))
            {
                throw new InvalidOperationException(
                    $"Duplicate interaction role '{roleType.Name}' detected for actor '{_ownerName}'. " +
                    $"Existing role owner: '{existingRole.GetType().Name}', new role owner: '{role.GetType().Name}'.");
            }

            _rolesByType.Add(roleType, role);
        }
    }
}