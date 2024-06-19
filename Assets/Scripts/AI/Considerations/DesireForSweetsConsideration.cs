using System;
using UnityEngine;

[Serializable]
public class DesireForSweetsConsideration : IUtilityConsideration
{
    [SerializeField] private AnimationCurve _desireForSweetsCurve;

    public float Score(AIContext context)
    {
        var desireForSweets = context.StatCollection.GetStat<DesireForSweets>();
        return _desireForSweetsCurve.Evaluate(desireForSweets.Value / desireForSweets.MaxValue);
    }
}