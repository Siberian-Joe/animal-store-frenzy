using System;
using UnityEngine;

[Serializable]
public class DesireForSaltyConsideration : IUtilityConsideration
{
    [SerializeField] private AnimationCurve _desireForSaltyCurve;

    public float Score(AIContext context)
    {
        var desireForSalty = context.StatCollection.GetStat<DesireForSalty>();
        return _desireForSaltyCurve.Evaluate(desireForSalty.Value / desireForSalty.MaxValue);
    }
}