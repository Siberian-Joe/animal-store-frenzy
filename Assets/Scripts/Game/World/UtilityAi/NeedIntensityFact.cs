using UnityEngine;

namespace Game.World.UtilityAi
{
    public readonly struct NeedIntensityFact
    {
        public NeedIntensityFact(float normalizedValue) => NormalizedValue = Mathf.Clamp01(normalizedValue);

        public float NormalizedValue { get; }
    }
}