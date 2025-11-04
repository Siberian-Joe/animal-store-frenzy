using R3;
using UnityEngine.AI;

namespace NewCore.Extensions
{
    public static class NavMeshAgentExtensions
    {
        public static Observable<Unit> WhenArrived(this NavMeshAgent agent)
        {
            return Observable.EveryUpdate()
                .Where(_ => agent.enabled && agent.pathPending == false)
                .Where(_ => agent.remainingDistance <= agent.stoppingDistance &&
                            (agent.hasPath == false || agent.velocity.sqrMagnitude == 0f))
                .Take(1)
                .AsUnitObservable();
        }
    }
}