using System;
using UnityEngine;

[Serializable]
public class HungerConsideration : IUtilityConsideration
{
    [SerializeField] private AnimationCurve _hungerCurve;

    public float Score(AIContext context)
    {
        var hunger = context.StatCollection.GetStat<Hunger>();
        return _hungerCurve.Evaluate(hunger.Value / hunger.MaxValue);
    }
}