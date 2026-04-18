using UnityEngine;

namespace Game.World.UtilityAi
{
    public readonly struct DistanceToTargetFact
    {
        public DistanceToTargetFact(float normalizedDistance) => NormalizedDistance = Mathf.Clamp01(normalizedDistance);

        public float NormalizedDistance { get; }
    }
}