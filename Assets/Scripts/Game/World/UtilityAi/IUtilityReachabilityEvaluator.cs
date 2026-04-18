using UnityEngine;

namespace Game.World.UtilityAi
{
    public interface IUtilityReachabilityEvaluator
    {
        bool TryEvaluate(
            Vector3 from,
            Vector3 to,
            out UtilityReachabilityEvaluation evaluation,
            out Vector3 projectedTo);
    }
}
