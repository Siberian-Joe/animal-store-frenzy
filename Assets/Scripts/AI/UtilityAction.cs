using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public abstract class UtilityAction : IUtilityAction
{
    [SerializeReference, SelectImplementation]
    private List<IUtilityConsideration> _considerations;

    [SerializeField] private float _weight = 1f;

    public float Evaluate(AIContext context, IActionScoreEvaluator scoreEvaluator)
    {
        var sb = new StringBuilder();
        sb.Append($"{GetType().Name}: ");
        return _weight * scoreEvaluator.EvaluateScore(context, _considerations, sb);
    }

    public abstract IEnumerator ExecuteCoroutine(UtilityBehaviour utilityBehaviour);
}