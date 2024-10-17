﻿using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class TakeSweetsAction : UtilityAction
{
    [SerializeField] private Transform _destination;

    public override IEnumerator ExecuteCoroutine(UtilityBehaviour utilityBehaviour)
    {
        var navMeshAgent = utilityBehaviour.NavMeshAgent;
        var desireForSweets = utilityBehaviour.StatCollection.GetStat<DesireForSweets>();
        var desireForCheckout = utilityBehaviour.StatCollection.GetStat<DesireToCheckout>();

        navMeshAgent.SetDestination(_destination.position);

        yield return new WaitUntil(() =>
            !navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance);

        yield return new WaitForSeconds(2);

        var value = desireForSweets.Value * Random.Range(0.15f, 0.5f);

        desireForSweets.Value -= value;
        desireForCheckout.Value += value;
    }
}