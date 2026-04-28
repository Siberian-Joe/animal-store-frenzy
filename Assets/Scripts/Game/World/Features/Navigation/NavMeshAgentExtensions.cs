using R3;
using UnityEngine.AI;

namespace Game.World.Features.Navigation
{
    public static class NavMeshAgentExtensions
    {
        public static Observable<Unit> WhenArrived(this NavMeshAgent agent)
        {
            return Observable.EveryUpdate()
                .Where(_ => agent && agent.enabled && agent.isOnNavMesh)
                .Where(_ => agent.pathPending == false)
                .Where(_ => agent.remainingDistance <= agent.stoppingDistance)
                .Where(_ => agent.hasPath == false || agent.velocity.sqrMagnitude <= 0.0001f)
                .Take(1)
                .AsUnitObservable();
        }
    }
}