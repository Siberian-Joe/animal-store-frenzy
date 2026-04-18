using UnityEngine;

namespace Game.World.UtilityAi
{
    public readonly struct UtilityReachabilityEvaluation
    {
        public bool IsReachable { get; }

        public float PathDistance { get; }

        public UtilityReachabilityEvaluation(bool isReachable, float pathDistance)
        {
            IsReachable = isReachable;
            PathDistance = Mathf.Max(0f, pathDistance);
        }

        public float NormalizeDistance(float maxRelevantDistance) =>
            Mathf.Clamp01(PathDistance / Mathf.Max(0.1f, maxRelevantDistance));

        public static UtilityReachabilityEvaluation Unreachable(float fallbackDistance) =>
            new(false, Mathf.Max(0f, fallbackDistance));
    }
}