using System;
using System.Collections.Generic;
using Game.World.Core;
using Game.World.Interactions;
using Game.World.Spatial;
using UnityEngine;

namespace Game.World.InteractionTarget
{
    public sealed class InteractionTargetFeature : EntityFeature, IInteractionTargetFeature
    {
        private readonly EntityRoot _targetRoot;
        private readonly ISpatialFeature _spatial;
        private readonly IReadOnlyList<IInteractionResolver> _resolvers;
        private readonly Vector3 _localInteractionOffset;

        public InteractionTargetFeature(
            EntityRoot targetRoot,
            ISpatialFeature spatial,
            IReadOnlyList<IInteractionResolver> resolvers,
            Vector3 localInteractionOffset)
        {
            _targetRoot = targetRoot ?? throw new ArgumentNullException(nameof(targetRoot));
            _spatial = spatial ?? throw new ArgumentNullException(nameof(spatial));
            _resolvers = resolvers ?? throw new ArgumentNullException(nameof(resolvers));
            _localInteractionOffset = localInteractionOffset;
        }

        public Vector3 ApproachPoint =>
            _spatial.Position.Value + _spatial.Rotation.Value * _localInteractionOffset;

        public bool TryResolve(
            EntityRoot initiator,
            out IEntityInteraction interaction)
        {
            foreach (var resolver in _resolvers)
            {
                if (resolver.TryResolve(initiator, _targetRoot, this, out interaction))
                    return true;
            }

            interaction = null;
            return false;
        }
    }
}