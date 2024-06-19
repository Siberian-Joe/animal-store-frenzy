using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class CheckoutAction : UtilityAction
{
    [SerializeField] private Transform _destination;

    public override IEnumerator ExecuteCoroutine(UtilityBehaviour utilityBehaviour)
    {
        var navMeshAgent = utilityBehaviour.NavMeshAgent;
        var desireForCheckout = utilityBehaviour.StatCollection.GetStat<DesireToCheckout>();

        navMeshAgent.SetDestination(_destination.position);

        yield return new WaitUntil(() =>
            !navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance);

        yield return new WaitForSeconds(5);

        desireForCheckout.Value = 0;

        Debug.Log("Checked out");
    }
}