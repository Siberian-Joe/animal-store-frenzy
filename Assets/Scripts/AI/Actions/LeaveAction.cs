using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class LeaveAction : UtilityAction
{
    [SerializeField] private Transform _destination;

    public override IEnumerator ExecuteCoroutine(UtilityBehaviour utilityBehaviour)
    {
        var navMeshAgent = utilityBehaviour.NavMeshAgent;

        navMeshAgent.SetDestination(_destination.position);

        yield return new WaitUntil(() =>
            !navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance);

        Debug.Log("Left the store");
    }
}