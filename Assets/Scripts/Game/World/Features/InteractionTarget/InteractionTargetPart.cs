using System;
using Game.World.EntityRuntime;
using Game.World.Interactions;
using Game.World.Features.Spatial;
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

        public bool TryBuildRequest(IInteractionRoleResolver source, out InteractionCommandRequest request)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            EnsureBuildersCached();

            foreach (var builder in _commandBuilders)
            {
                if (builder.TryBuild(source, ApproachPoint, out request))
                    return true;
            }

            request = null;
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
                    $"Interaction target '{name}' has no local interaction command builders.");
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
    }
}
