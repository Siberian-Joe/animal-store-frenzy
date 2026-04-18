using System;
using System.Collections.Generic;
using Game.World.EntityRuntime;
using Game.World.Features.Spatial;
using Game.World.Interactions;
using UnityEngine;

namespace Game.World.Features.InteractionTarget
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SpatialPart))]
    public class InteractionTargetPart : EntityComponent
    {
        [SerializeField] private Vector3 _localInteractionOffset = new(0f, 0f, -0.75f);

        [Header("References")] [SerializeField]
        private SpatialPart _spatial;

        private InteractionCommandBuilderPart[] _commandBuilders;

        public override int ActivationOrder => 400;

        public Vector3 ApproachPoint =>
            Spatial.Position.Value + Spatial.Rotation.Value * _localInteractionOffset;

        public void CollectOptions(IInteractionActor actor, List<InteractionOption> options)
        {
            if (actor == null)
                throw new ArgumentNullException(nameof(actor));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            EnsureBuildersCached();

            foreach (var builder in _commandBuilders)
                builder.CollectOptions(actor, ApproachPoint, options);
        }

        public bool TryGetPrimaryOption(IInteractionActor actor, out InteractionOption option)
        {
            if (actor == null)
                throw new ArgumentNullException(nameof(actor));

            EnsureBuildersCached();
            var optionsBuffer = InteractionTargetBuffers.Get();

            for (var index = 0; index < _commandBuilders.Length; index++)
            {
                var startCount = optionsBuffer.Count;
                _commandBuilders[index].CollectOptions(actor, ApproachPoint, optionsBuffer);

                if (optionsBuffer.Count > startCount)
                {
                    option = optionsBuffer[startCount];
                    optionsBuffer.Clear();
                    return true;
                }
            }

            optionsBuffer.Clear();
            option = null;
            return false;
        }

        protected override void OnActivate()
        {
            if (Spatial == false)
            {
                throw new InvalidOperationException(
                    $"{nameof(InteractionTargetPart)} on '{name}' requires {nameof(SpatialPart)}.");
            }

            EnsureBuildersCached();

            if (_commandBuilders.Length == 0)
            {
                throw new InvalidOperationException(
                    $"Interaction target '{name}' has no local interaction option builders.");
            }
        }

        private SpatialPart Spatial
        {
            get
            {
                _spatial ??= GetComponent<SpatialPart>();
                return _spatial;
            }
        }

        private void EnsureBuildersCached()
        {
            if (_commandBuilders != null)
                return;

            var builders = GetComponents<InteractionCommandBuilderPart>();
            Array.Sort(builders, static (left, right) => left.Order.CompareTo(right.Order));
            _commandBuilders = builders;
        }

        private static class InteractionTargetBuffers
        {
            [ThreadStatic] private static List<InteractionOption> _options;

            public static List<InteractionOption> Get() => _options ??= new List<InteractionOption>(4);
        }
    }
}
