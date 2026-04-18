using UnityEngine;
using UnityEngine.AI;

namespace Game.World.UtilityAi
{
    public sealed class NavMeshUtilityReachabilityEvaluator : IUtilityReachabilityEvaluator
    {
        private const float SampleDistance = 2f;

        private readonly NavMeshPath _path = new();
        private readonly Vector3[] _cornerBuffer = new Vector3[64];

        public bool TryEvaluate(
            Vector3 from,
            Vector3 to,
            out UtilityReachabilityEvaluation evaluation,
            out Vector3 projectedTo)
        {
            if (TrySamplePosition(from, out var projectedFrom) == false ||
                TrySamplePosition(to, out projectedTo) == false)
            {
                evaluation = UtilityReachabilityEvaluation.Unreachable(
                    Vector3.Distance(from, to));
                projectedTo = default;
                return false;
            }

            if (NavMesh.CalculatePath(projectedFrom, projectedTo, NavMesh.AllAreas, _path) == false)
            {
                evaluation = UtilityReachabilityEvaluation.Unreachable(
                    Vector3.Distance(projectedFrom, projectedTo));
                return false;
            }

            var pathDistance = CalculatePathDistance(_path);
            var isReachable = _path.status == NavMeshPathStatus.PathComplete;

            evaluation = new UtilityReachabilityEvaluation(isReachable, pathDistance);
            return true;
        }

        private static bool TrySamplePosition(Vector3 position, out Vector3 projectedPosition)
        {
            if (NavMesh.SamplePosition(position, out var hit, SampleDistance, NavMesh.AllAreas))
            {
                projectedPosition = hit.position;
                return true;
            }

            projectedPosition = default;
            return false;
        }

        private float CalculatePathDistance(NavMeshPath path)
        {
            var cornerCount = path.GetCornersNonAlloc(_cornerBuffer);

            if (cornerCount <= 1)
                return 0f;

            if (cornerCount < _cornerBuffer.Length)
                return SumDistance(_cornerBuffer, cornerCount);

            var corners = path.corners;
            return SumDistance(corners, corners.Length);
        }

        private static float SumDistance(Vector3[] corners, int count)
        {
            var total = 0f;

            for (var index = 1; index < count; index++)
                total += Vector3.Distance(corners[index - 1], corners[index]);

            return total;
        }
    }
}
