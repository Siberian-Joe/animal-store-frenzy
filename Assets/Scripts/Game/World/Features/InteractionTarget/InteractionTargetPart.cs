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
        private InteractionApproachPointPart[] _approachPointProviders;

        public override int ActivationOrder => 400;

        public Vector3 ApproachPoint => ResolveLocalOffset(_localInteractionOffset);

        public Vector3 ResolveApproachPoint(IInteractionActor actor)
        {
            if (actor == null)
                throw new ArgumentNullException(nameof(actor));

            EnsureApproachPointProvidersCached();

            InteractionApproachPointPart matchedProvider = null;

            for (var index = 0; index < _approachPointProviders.Length; index++)
            {
                var provider = _approachPointProviders[index];
                if (provider == false || provider.isActiveAndEnabled == false || provider.Supports(actor) == false)
                    continue;

                if (matchedProvider != false)
                {
                    throw new InvalidOperationException(
                        $"Interaction target '{name}' has multiple approach point providers matching actor " +
                        $"'{actor.GetType().Name}': '{matchedProvider.GetType().Name}' and '{provider.GetType().Name}'.");
                }

                matchedProvider = provider;
            }

            return matchedProvider != false
                ? matchedProvider.Resolve(Spatial)
                : ApproachPoint;
        }

        public void CollectOptions(IInteractionActor actor, List<InteractionOption> options)
        {
            if (actor == null)
                throw new ArgumentNullException(nameof(actor));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            EnsureBuildersCached();
            var approachPoint = ResolveApproachPoint(actor);

            foreach (var builder in _commandBuilders)
                builder.CollectOptions(actor, approachPoint, options);
        }

        public bool TryGetPrimaryOption(IInteractionActor actor, out InteractionOption option)
        {
            if (actor == null)
                throw new ArgumentNullException(nameof(actor));

            EnsureBuildersCached();
            var approachPoint = ResolveApproachPoint(actor);
            var optionsBuffer = InteractionTargetBuffers.Get();

            for (var index = 0; index < _commandBuilders.Length; index++)
            {
                var startCount = optionsBuffer.Count;
                _commandBuilders[index].CollectOptions(actor, approachPoint, optionsBuffer);

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
            EnsureApproachPointProvidersCached();

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

        private Vector3 ResolveLocalOffset(Vector3 localOffset) =>
            Spatial.Position.Value + Spatial.Rotation.Value * localOffset;

        private void EnsureBuildersCached()
        {
            if (_commandBuilders != null)
                return;

            var builders = GetComponents<InteractionCommandBuilderPart>();
            Array.Sort(builders, static (left, right) => left.Order.CompareTo(right.Order));
            _commandBuilders = builders;
        }

        private void EnsureApproachPointProvidersCached()
        {
            if (_approachPointProviders != null)
                return;

            _approachPointProviders = GetComponents<InteractionApproachPointPart>();
        }

        private void OnDrawGizmosSelected()
        {
            var origin = transform.position;
            var point = origin + transform.rotation * _localInteractionOffset;

            Gizmos.color = Color.white;
            Gizmos.DrawLine(origin, point);
            Gizmos.DrawWireSphere(point, 0.1f);
        }

        private static class InteractionTargetBuffers
        {
            [ThreadStatic] private static List<InteractionOption> _options;

            public static List<InteractionOption> Get() => _options ??= new List<InteractionOption>(4);
        }
    }
}