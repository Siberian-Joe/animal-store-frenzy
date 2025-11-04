using System;
using NewCore.Modules.Interaction.Abstractions;
using NewCore.Modules.Interaction.Context;

namespace NewCore.Modules.Interaction.Rules
{
    public abstract class InteractionRule<TTarget> : IInteractionRule
        where TTarget : IActor
    {
        private readonly Type _type = typeof(TTarget);

        protected abstract bool Filter(IActor initiator, TTarget target);

        protected abstract void Execute(IActor initiator, TTarget target);

        public bool TryApply(InteractionContext context)
        {
            if (_type != context.Target.GetType())
                return false;

            var initiator = context.Initiator;
            var target = (TTarget)context.Target;

            if (Filter(initiator, target) == false)
                return false;

            Execute(initiator, target);
            return true;
        }
    }
}