using UnityEngine;

namespace Game.World.UtilityAi
{
    public readonly struct SelfHealthFact
    {
        public SelfHealthFact(float normalizedValue) => NormalizedValue = Mathf.Clamp01(normalizedValue);

        public float NormalizedValue { get; }
    }
}