using System;
using System.Reflection;

namespace Game.World.Persistence
{
    public sealed class ReflectionEntityStateFactory : IEntityStateFactory
    {
        public IEntityStateData Create(Type stateType)
        {
            if (stateType == null)
                throw new ArgumentNullException(nameof(stateType));

            if (typeof(IEntityStateData).IsAssignableFrom(stateType) == false)
            {
                throw new InvalidOperationException(
                    $"State type '{stateType.Name}' does not implement {nameof(IEntityStateData)}.");
            }

            try
            {
                if (Activator.CreateInstance(stateType) is not IEntityStateData created)
                {
                    throw new InvalidOperationException(
                        $"Infrastructure could not create state slice '{stateType.Name}'.");
                }

                return created;
            }
            catch (Exception exception) when (
                exception is MissingMethodException or MemberAccessException or TargetInvocationException)
            {
                throw new InvalidOperationException(
                    $"State slice '{stateType.Name}' must be constructible by infrastructure.",
                    exception);
            }
        }
    }
}