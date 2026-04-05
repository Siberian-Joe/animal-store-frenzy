using Game.World.Composition;
using Game.World.Interactions;
using R3;
using UnityEngine;

namespace Game.World.InteractionTarget
{
    [DisallowMultipleComponent]
    public class InteractionTargetPart : FeaturePart<IInteractionTargetFeature>
    {
        [SerializeField] private Vector3 _localInteractionOffset = new(0f, 0f, -0.75f);

        public Vector3 LocalInteractionOffset => _localInteractionOffset;

        public override void Bind(IInteractionTargetFeature feature, CompositeDisposable disposables)
        {
        }
    }
}
