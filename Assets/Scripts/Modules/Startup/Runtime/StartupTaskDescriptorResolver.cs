using System;
using System.Collections.Generic;
using Modules.Startup.Contracts;

namespace Modules.Startup.Runtime
{
    public sealed class StartupTaskDescriptorResolver : IStartupTaskDescriptorResolver
    {
        private static readonly Dictionary<Type, StartupAttribute> Cache = new();

        public StartupTaskDescriptor Resolve(IStartupTask task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            var type = task.GetType();

            if (Cache.TryGetValue(type, out var attribute) == false)
            {
                attribute = (StartupAttribute)Attribute.GetCustomAttribute(type, typeof(StartupAttribute));

                Cache[type] = attribute ?? throw new InvalidOperationException(
                    $"Startup task '{type.FullName}' must have [{nameof(StartupAttribute)}].");
            }

            var name = string.IsNullOrWhiteSpace(task.Name) ? type.Name : task.Name;

            return new StartupTaskDescriptor(
                task,
                type,
                name,
                attribute.Phase,
                attribute.Order,
                attribute.FailurePolicy,
                attribute.ExecutionMode);
        }
    }
}